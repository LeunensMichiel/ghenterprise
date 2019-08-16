using Ghenterprise.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Ghenterprise.Data
{
    class UserService:BaseService
    {
        public Task<HttpResponseMessage> RegisterUser(User user)
        {
            return Post("/User/register", JsonConvert.SerializeObject(user));
        }
    }
}
