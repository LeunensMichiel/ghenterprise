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
            return await PostAsync("Enterprise", enterprise);
        }

        public async Task<List<Enterprise>> GetEnterprisesAsync(bool refresh = true)
        {
            return await GetAsync<List<Enterprise>>("Enterprise", forceRefresh:refresh);
        }

        public async Task<List<Enterprise>> GetEnterprisesByOwner(bool refresh = true)
        {
            return await GetAsync<List<Enterprise>>("Enterprise/Owner", forceRefresh: refresh);
        }

        public async Task<List<Enterprise>> GetEnterpriseAsync(string Id)
        {
            return await GetAsync<List<Enterprise>>($"Enterprise?enterprise_id={Id}");
        }

        public async Task<bool> UpdateEnterprise(Enterprise enterprise)
        {
            return await PutAsync("Enterprise", enterprise);
        }

        public async Task<bool> DeleteEnterprise(string Id)
        {
            return await DeleteAsync($"Enterprise?enterprise_id={Id}");
        }

        public async Task<bool> SubscribeToEnterprise(string Id)
        {
            return await PostAsync($"Enterprise?entID={Id}", Id);
        }

        public async Task<bool> UnSubscribeToEnterprise(string Id)
        {
            return await DeleteAsync($"Enterprise?subscription_id={Id}");
        }
        public async Task<List<Enterprise>> GetSubscriptionsAsync(bool refresh = true)
        {
            return await GetAsync<List<Enterprise>>("Enterprise/Subscription", forceRefresh: refresh);
        }
        public async Task<List<Event>> GetSubscriptionsEventsAsync(bool refresh = true)
        {
            return await GetAsync<List<Event>>("Event/Subscription", forceRefresh: refresh);
        }

        public async Task<List<Promotion>> GetSubscriptionsPromosAsync(bool refresh = true)
        {
            return await GetAsync<List<Promotion>>("Promotion/Subscription", forceRefresh: refresh);
        }
    }
}
