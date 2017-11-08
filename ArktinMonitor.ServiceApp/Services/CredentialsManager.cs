using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using CredentialManagement;

namespace ArktinMonitor.ServiceApp.Services
{
    internal static class CredentialsManager
    {
        private const string PasswordName = "ArktinMonitor";

        public static void SavePassword(SecureString password)
        {
            using (var credential = new Credential  ())
            {
                credential.Target = PasswordName;
                credential.PersistanceType = PersistanceType.LocalComputer;
                credential.Password = new NetworkCredential("Arktin", password).Password;
                credential.Save();
            }
        }

        public static SecureString GetPassword()
        {
            using (var credential = new Credential())
            {
                credential.Target = PasswordName;
                credential.Load();
                return credential.SecurePassword;
            }
        }


    }
}
