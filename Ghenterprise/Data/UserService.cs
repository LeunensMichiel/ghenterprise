using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Ghenterprise.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Web;

namespace Ghenterprise.Data
{
    public class UserService : BaseService
    {

        public async Task<User> PostRegisterUser(User user)
        {
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(user));
            var content = new StringContent(stringPayload.ToString(), Encoding.UTF8, "application/json");
            
            var response = await Client.PostAsync(GetRequestUri("/User/register"), content);
            
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            User dbUser = JsonConvert.DeserializeObject<User>(result);
            if (!string.IsNullOrEmpty(dbUser.Token))
            {
                SetToken(dbUser);
                return dbUser;
            }
            return null;
        }

        public async Task<string> GetCheckEmail(User user)
        { 
            var response = await Client.GetAsync(GetRequestUri(String.Format("{0}?email={1}", "/User/check-email", user.Email)));

            var result = await response.Content.ReadAsStringAsync();
            string token = JsonConvert.DeserializeObject<User>(result).Token;
            return token ;
        }

        public async Task<User> PostLogin(User user)
        {
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(user));
            var content = new StringContent(stringPayload.ToString(), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(GetRequestUri("/User/login"), content);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            User dbUser = JsonConvert.DeserializeObject<User>(result);
            if (!string.IsNullOrEmpty(dbUser.Token))
            {
                SetToken(dbUser);
                return dbUser;
            }
            return null;
        }

        public void SetToken(User user)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            vault.Add(new Windows.Security.Credentials.PasswordCredential("User", user.Firstname, user.Token));
        }

        public void RemoveToken(User user)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            vault.Remove(new Windows.Security.Credentials.PasswordCredential("User", user.Firstname, user.Token));
        }

        public Windows.Security.Credentials.PasswordCredential GetCredentialFromLocker()
        {
            Windows.Security.Credentials.PasswordCredential credential = null;

            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource("User");
            if (credentialList.Count > 0)
            {
                if (credentialList.Count == 1)
                {
                    credential = credentialList[0];
                }
            }

            return credential;
        }
    }
}
