using Ghenterprise.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Data
{
    public class EnterpriseService:BaseService
    {
        public async Task<bool> SaveEnterprise( Enterprise enterprise)
        {
            //var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(enterprise));
            //var content = new StringContent(stringPayload.ToString(), Encoding.UTF8, "application/json");
            ////https://localhost:44307/api/Enterprise?UserID=1
            //var response = await Client.PostAsync(String.Format("{0}?UserID={1}", GetRequestUri("/Enterprise"), userId), content);

            //response.EnsureSuccessStatusCode();

            //var result = await response.Content.ReadAsStringAsync();
            return await PostAsync("Enterprise", enterprise);
        }

        public async Task<List<Enterprise>> GetEnterprisesAsync()
        {
            return await GetAsync<List<Enterprise>>("Enterprise");
        }
    }
}
