using ArktinMonitor.Data.Models;
using CredentialManagement;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security;

namespace ArktinMonitor.Helpers
{
    public class CredentialsManager
    {
        private readonly string _url;

        private readonly string _passwordName;

        private readonly string _userRelatedPath;

        private readonly string _systemRelatedPath;

        /// <summary>
        /// Manager to storing and editing credentials like, password, email and json web token.
        /// Also generates json web token when needed.
        /// </summary>
        /// <param name="url">URL address of web api</param>
        /// <param name="userRelatedPath">AppData folder</param>
        /// <param name="systemRelatedPath">ProgramData path</param>
        /// <param name="passwordName">Name for the credential to save using Windows Credentials</param>
        public CredentialsManager(string url, string userRelatedPath, string systemRelatedPath, string passwordName)
        {
            _url = url;
            _userRelatedPath = userRelatedPath;
            _systemRelatedPath = systemRelatedPath;
            _passwordName = passwordName;
        }

        /// <summary>
        /// Using possibly stored bearer token or web api credentials tries to get token.
        /// </summary>
        /// <returns>true if successful, false otherwise.</returns>
        public bool TryGetWebApiToken()
        {
            var access = CheckWebApiAccess();
            if (access || !AreCredentialsStored()) return access;
            var password = new NetworkCredential("arktin", GetPassword()).Password;
            var token = RenewBearerToken(GetEmail(), password);
            if (token != null)
            {
                access = true;
            }
            return access;
        }

        /// <summary>
        /// Looks for stored bearer token and checks it against web api
        /// </summary>
        /// <returns>true if bearer token is found and correct, false otherwise</returns>
        public bool CheckWebApiAccess()
        {
            var jwt = LoadJsonWebToken()?.AccessToken ?? string.Empty;
            return CheckJsonWebToken(jwt);
        }

        /// <summary>
        /// Stores both email and password in email.an file located in selected path.
        /// </summary>
        /// <param name="email">Email address to save</param>
        /// <param name="password">Password to save</param>
        public void StoreCredentials(string email, string password)
        {
            JsonHelper.SerializeToJsonFile(Path.Combine(_userRelatedPath, "email.an"), email);
            SavePassword(password);
        }

        /// <summary>
        /// Saves (or overrdes last stored) password.
        /// </summary>
        /// <param name="password">Pasword to save</param>
        public void SavePassword(string password)
        {
            using (var credential = new Credential())
            {
                credential.Target = _passwordName;
                credential.PersistanceType = PersistanceType.LocalComputer;
                credential.Password = password;
                credential.Save();
            }
        }

        /// <summary>
        /// Returns last saved password.
        /// </summary>
        /// <returns>Last saved password</returns>
        public SecureString GetPassword()
        {
            using (var credential = new Credential())
            {
                credential.Target = _passwordName;
                credential.Load();
                return credential.SecurePassword;
            }
        }

        /// <summary>
        /// Removes completly any saved password.
        /// </summary>
        public void RemovePassword()
        {
            SavePassword(string.Empty);
        }

        /// <summary>
        /// Replaces stored email address with empty string.
        /// </summary>
        public void RemoveEmail()
        {
            JsonHelper.SerializeToJsonFile(Path.Combine(_userRelatedPath, "email.an"), string.Empty);
        }
        /// <summary>
        /// Returns Last stored email address from email.an file.
        /// </summary>
        /// <returns>Last stored email address</returns>
        public string GetEmail()
        {
            var email = JsonHelper.DeserializeJson<string>(Path.Combine(_userRelatedPath, "email.an"));
            return email ?? string.Empty;
        }

        /// <summary>
        /// Tries to return JWT string.
        /// </summary>
        /// <returns>String containing JWT</returns>
        public string GetJsonWebToken()
        {
            var jwt = LoadJsonWebToken();
            var correct = jwt != null && CheckJsonWebToken(jwt.AccessToken);
            if (!string.IsNullOrWhiteSpace(jwt?.AccessToken) && correct) return jwt.AccessToken;
            if (!AreCredentialsStored())
            {
                throw new Exception("Credentials not found");
            }
            jwt = RenewBearerToken(GetEmail(), new NetworkCredential("arktin", GetPassword()).Password);
            return jwt.AccessToken;
        }

        /// <summary>
        /// Store Json Web Token in a arktin.an json file.
        /// </summary>
        /// <param name="token">Json Web Token object</param>
        private void StoreJsonWebToken(TokenResponse token)
        {
            JsonHelper.SerializeToJsonFile(Path.Combine(_systemRelatedPath, "arktin.an"), token);
        }

        /// <summary>
        /// Loads Json Web Token from the target location.
        /// </summary>
        /// <returns>Json Web Token</returns>
        private TokenResponse LoadJsonWebToken()
        {
            return JsonHelper.DeserializeJson<TokenResponse>(Path.Combine(_systemRelatedPath, "arktin.an"));
        }

        /// <summary>
        /// Sends a request to Web Api to generate Json Web Token, then saves it in json file.
        /// </summary>
        /// <param name="email">Email of existing account</param>
        /// <param name="password">Password of existing account</param>
        public TokenResponse RenewBearerToken(string email, string password)
        {
            var body = new StringContent($"username={email}&password={password}&grant_type=password");
            try
            {
                using (var client = new HttpClient())
                {
                    //LocalLogger.Log("Authorization started");
                    var response = client.PostAsync($"{_url}token", body).Result;
                    //LocalLogger.Log(response.Content.ReadAsStringAsync().Result);
                    var content = response.Content.ReadAsAsync<TokenResponse>().Result;
                    LocalLogger.Log("Token: " + content.AccessToken);
                    StoreJsonWebToken(content);
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
        /// <param name="jsonWebToken">Json Web Token</param>
        /// <returns></returns>
        public bool CheckJsonWebToken(string jsonWebToken)
        {
            var client = new ServerClient();
            var response = client.GetFromServer(_url, "api/checkAccess", jsonWebToken);
            LocalLogger.Log(response.Content.ReadAsStringAsync().Result);
            return response.Content.ReadAsAsync<bool>().Result;
        }

        /// <summary>
        /// Checks if there are both email and password already stored on the computer.
        /// </summary>
        /// <returns>true if email and password are found, false otherwise</returns>
        public bool AreCredentialsStored()
        {
            var pass = new NetworkCredential("Arktin", GetPassword()).Password;
            var email = GetEmail();
            return !string.IsNullOrWhiteSpace(pass) && !string.IsNullOrWhiteSpace(email);
        }
    }
}