using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using ArktinMonitor.Data.Models;
using CredentialManagement;
using Newtonsoft.Json;

namespace ArktinMonitor.Helpers
{
    public static class CredentialsManager
    {
        private const string PasswordName = "ArktinMonitor";
        private static readonly string LocalStoragePath = Environment.UserInteractive
            ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
            : AppDomain.CurrentDomain.BaseDirectory;

        public static bool CheckWebApiAccess(string url, string path)
        {
            var jwt = LoadJsonWebToken(path)?.AccessToken ?? string.Empty;
            return CheckJsonWebToken(url, jwt);
        }

        /// <summary>
        /// Stores both email and password in email.an file located in selected path.
        /// </summary>
        /// <param name="path">path toemail.an file</param>
        /// <param name="email">Email address to save</param>
        /// <param name="password">Password to save</param>
        public static void StoreCredentials(string path, string email, string password)
        {
            JsonHelper.SerializeToJsonFile(Path.Combine(path, "email.an"), email);
            SavePassword(password);
        }

        /// <summary>
        /// Saves (or overrdes last stored) password.
        /// </summary>
        /// <param name="password">Pasword to save</param>
        public static void SavePassword(string password)
        {
            using (var credential = new Credential())
            {
                credential.Target = PasswordName;
                credential.PersistanceType = PersistanceType.LocalComputer;
                credential.Password = password;
                credential.Save();
            }
        }

        /// <summary>
        /// Returns last saved password.
        /// </summary>
        /// <returns>Last saved password</returns>
        public static SecureString GetPassword()
        {
            using (var credential = new Credential())
            {
                credential.Target = PasswordName;
                credential.Load();
                return credential.SecurePassword;
            }
        }

        /// <summary>
        /// Removes completly any saved password.
        /// </summary>
        public static void PurgePassword()
        {
            SavePassword(string.Empty);
        }
        /// <summary>
        /// Returns Last stored email address from email.an file.
        /// </summary>
        /// <returns>Last stored email address</returns>
        public static string GetEmail()
        {
            return JsonHelper.DeserializeJson<string>(Path.Combine(LocalStoragePath, "email.an"));
        }

        /// <summary>
        /// Tries to return JWT string.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>String containing JWT</returns>
        public static string GetJsonWebToken(string url)
        {
            if (!AreCredentialsStored()) return null;
            var jwt = LoadJsonWebToken(LocalStoragePath);

            if (jwt != null)
            {
                CheckJsonWebToken(url, jwt.AccessToken);
            }
            return null;
        }

        /// <summary>
        /// Store Json Web Token in a arktin.an json file.
        /// </summary>
        /// <param name="path">Path to destination folder</param>
        /// <param name="token">Json Web Token object</param>
        private static void StoreJsonWebToken(string path, TokenResponse token)
        {
            JsonHelper.SerializeToJsonFile(Path.Combine(path, "arktin.an"), token);
        }

        /// <summary>
        /// Loads Json Web Token from the target location.
        /// </summary>
        /// <param name="path">Location of the folder containing JWT arktin.an file</param>
        /// <returns>Json Web Token</returns>
        private static TokenResponse LoadJsonWebToken(string path)
        {
            return JsonHelper.DeserializeJson<TokenResponse>(Path.Combine(path, "arktin.an"));
        }

        /// <summary>
        /// Sends a request to Web Api to generate Json Web Token, then saves it in json file.
        /// </summary>
        /// <param name="url">Url of the WebApp</param>
        /// <param name="email">Email of existing account</param>
        /// <param name="password">Password of existing account</param>
        public static TokenResponse RenewBearerToken(string url, string email, string password)
        {
            var body = new StringContent($"username={email}&password={password}&grant_type=password");
            try
            {
                using (var client = new HttpClient())
                {
                    //LocalLogger.Log("Authorization started");
                    var response = client.PostAsync($"{url}token", body).Result;
                    //LocalLogger.Log(response.Content.ReadAsStringAsync().Result);
                    var content = response.Content.ReadAsAsync<TokenResponse>().Result;
                    LocalLogger.Log("Token: " + content.AccessToken);
                    StoreJsonWebToken(LocalStoragePath, content);
                    return content;
                }
            }
            catch (Exception e)
            {
                LocalLogger.Log("Authorization", e);
                return null;
            }
        }
        /// <summary>
        /// Validates Json Web Token.
        /// </summary>
        /// <param name="url">Address of web app</param>
        /// <param name="jsonWebToken">Json Web Token</param>
        /// <returns></returns>
        public static bool CheckJsonWebToken(string url, string jsonWebToken)
        {
            var client = new ServerClient();
            var response = client.GetFromServer(url, "api/checkAccess", jsonWebToken);
            LocalLogger.Log(response.Content.ReadAsStringAsync().Result);
            return response.Content.ReadAsAsync<bool>().Result;
        }

        /// <summary>
        /// Checks if there are both email and password already stored on the computer.
        /// </summary>
        /// <returns>true if email and password are found, false otherwise</returns>
        public static bool AreCredentialsStored()
        {
            var pass = new NetworkCredential("Arktin", GetPassword()).Password;
            var email = GetEmail();
            return !string.IsNullOrWhiteSpace(pass) && !string.IsNullOrWhiteSpace(email);
        }
    }
}
