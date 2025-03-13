using System.Text;

namespace Admin
{
    public class HttpConnector
    {
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = new HttpClient { Timeout = TimeSpan.FromMinutes(80) };

            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, string json)
        {
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient { Timeout = TimeSpan.FromMinutes(80) };

            var response = await client.PostAsync(url, data);

            return response;
        }
    }
}