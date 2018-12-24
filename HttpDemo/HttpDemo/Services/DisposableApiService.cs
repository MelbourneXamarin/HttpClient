using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpDemo
{
    public class DisposableApiService : IDisposableApiService
    {
        //Url doesn't exist.
        public const string myUrl = "http://url.doesnt.exist.com/api/";

        public async Task<T> Get<T>(string url) where T : class
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"{myUrl}{url}");
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<T> Post<T>(string url, object data) where T : class
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    var req = new HttpRequestMessage(HttpMethod.Post, $"{myUrl}{url}");
                    if (data is HttpContent)
                        req.Content = data as HttpContent;
                    else
                        req.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                    var response = await httpClient.SendAsync(req);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }
        }
    }
}