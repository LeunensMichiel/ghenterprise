using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Data
{
    class BaseService
    {
        internal HttpClient Client { get; set; }
        private readonly string baseAdress = "https://localhost:44307/api";

        public BaseService()
        {
            Client = new HttpClient(
                    new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (a, b, c, d) => true
                    }
                ); 
        }

        internal String GetRequestUri(String route)
        {
            return baseAdress + route;
        }

        internal async Task<StringContent> ObjectToStringContent<T>(T t)
        {
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(t));
            var content = new StringContent(stringPayload.ToString(), Encoding.UTF8, "application/json");

            return content;
        }
    }
}
