using Ghenterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Data
{
    public class PromotionService : BaseService
    {
        public PromotionService()
        {

        }

        public async Task<bool> SavePromoAsync(Promotion promo)
        {
            return await PostAsync("Promotion", promo);
        }

        public async Task<List<Promotion>> GetPromosAsync()
        {
            return await GetAsync<List<Promotion>>("Promotion", true);
        }

        public async Task<Promotion> GetPromotionById(string prom_id)
        {
            return await GetAsync<Promotion>($"Promotion?Promotion_ID={prom_id}");
        }

        public async Task<List<Promotion>> GetPromosOfOwner()
        {
            return await GetAsync<List<Promotion>>("Promotion/Owner", true);
        }

        public async Task<bool> UpdatePromotion(Promotion prom)
        {
            return await PutAsync("Promotion", prom);
        }

        public async Task<bool> DeletePromotion(string Prom_ID)
        {
            return await DeleteAsync($"Promotion?Promotion_ID={Prom_ID}");
        }

        public async Task<Promotion> GetPromoAsync(string Id)
        {
            return await GetAsync<Promotion>($"Promotion?Promotion_ID={Id}", true);
        }
    }
}
