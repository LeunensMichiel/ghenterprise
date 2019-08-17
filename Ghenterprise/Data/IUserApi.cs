using Ghenterprise.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Ghenterprise.Data
{
    public interface IUserApi
    {
        [Post("/User/register")]
        Task<int> Register([Body] User user);

    }
}
