using Kulman.WPA81.BaseRestService.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Data
{
    public class BaseService : BaseRestService
    {
        private string _baseUrl = "https://localhost:44307/api";

        protected override string GetBaseUrl()
        {
            return _baseUrl;
        }
    }
}
