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
    }
}
