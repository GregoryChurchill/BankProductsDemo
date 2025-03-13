using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingProductsApi
{
    public class HttpConnector
    {
        public async Task<HttpResponseMessage> GetProductsAsync(string url, string version, string minVersion)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("x-v", version);
            request.Headers.Add("x-min-v", minVersion);

            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(500)
            };

            return await client.SendAsync(request);
        }
    }
}