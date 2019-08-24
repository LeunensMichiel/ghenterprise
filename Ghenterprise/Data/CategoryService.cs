using Ghenterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Data
{
    public class CategoryService:BaseService
    {
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await GetAsync<List<Category>>("Category");
        }
    }
}
