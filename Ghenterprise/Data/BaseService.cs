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
    public class BaseService
    {
        internal HttpClient Client { get; set; }
        private readonly string baseAdress = "https://localhost:44307/api/";
        private readonly Dictionary<string, object> responseCache;

        public BaseService()
        {
            Client = new HttpClient(
                    new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (a, b, c, d) => true
                    }
                );

            if (!string.IsNullOrEmpty(baseAdress))
            {
                Client.BaseAddress = new Uri($"{baseAdress}/");
            }

            responseCache = new Dictionary<string, object>();
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

        public async Task<T> GetAsync<T>(string uri, bool forceRefresh = false)
        {
            T result = default(T);

            if (forceRefresh || !responseCache.ContainsKey(uri))
            {
               
                HttpResponseMessage response = await Client.GetAsync(GetRequestUri(uri));

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("{0} returned {1}", GetRequestUri(uri), response.StatusCode));
                }

                var json = await response.Content.ReadAsStringAsync();
                result = await Task.Run(() => JsonConvert.DeserializeObject<T>(
                    json,
                    new IsoDateTimeConverter
                    {
                        DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff"
                    }
                ));

                if (responseCache.ContainsKey(uri))
                {
                    responseCache[uri] = result;
                }
                else
                {
                    responseCache.Add(uri, result);
                }
            }
            else
            {
                result = (T)responseCache[uri];
            }

            return result;
        }

        public async Task<bool> PostAsync<T>(string uri, T item)
        {
            if (item == null)
            {
                return false;
            }
            Client.DefaultRequestHeaders.Add("username", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImhvSmlyNDdVcTg5ciIsIm5iZiI6MTU2NjUwMDA4MCwiZXhwIjoxNTY3MTA0ODgwLCJpYXQiOjE1NjY1MDAwODB9.LkHgGgembArNR40ePNCMzHbMGeSb4YwLGLlCo4YY-Jg");

            var stringPayload = JsonConvert.SerializeObject(item, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(GetRequestUri(uri), content);

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> PostAsJsonAsync<T>(string uri, T item)
        {
            if (item == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await Client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync<T>(string uri, T item)
        {
            if (item == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await Client.PutAsync(uri, byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsJsonAsync<T>(string uri, T item)
        {
            if (item == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await Client.PutAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string uri)
        {
            var response = await Client.DeleteAsync(uri);

            return response.IsSuccessStatusCode;
        }
    }
}
