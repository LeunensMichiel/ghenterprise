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

        public async Task<int> PostRegisterUser(User user)
        {
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(user));
            var content = new StringContent(stringPayload.ToString(), Encoding.UTF8, "application/json");
            
            var response = await Client.PostAsync(GetRequestUri("/User/register"), content);
            
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return int.Parse(result);
        }

        public async Task<Response> GetCheckEmail(String email)
        { 
            var response = await Client.GetAsync(GetRequestUri(String.Format("{0}?email={1}", "/User/check-email", email)));

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response>(result);
        }

        public async Task<Response> PostLogin(User user)
        {
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(user));
            var content = new StringContent(stringPayload.ToString(), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(GetRequestUri("/User/login"), content);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response>(result);
        }
    }
}
