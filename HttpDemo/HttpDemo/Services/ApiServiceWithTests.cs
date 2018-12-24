using System.Net.Http;
using System.Threading.Tasks;
using HttpDemo.HttpMessageHandlers;
using ModernHttpClient;
using Newtonsoft.Json;

namespace HttpDemo.Services
{
    public class ApiServiceWithTests
    {
        private readonly HttpClient httpClient;

        //3. modernHttpClient demo
        public ApiServiceWithTests() : this(new NativeMessageHandler()) { }

        public ApiServiceWithTests(HttpMessageHandler handler)
        {            
            //1. Native project defaults
            //this.httpClient = new HttpClient()

            //2. HttpClient with standard httpclienthandler
            //httpClient = new HttpClient(new StandardHttpClientHandler())

            //3. modernHttpClient demo
            //httpClient = new HttpClient(new NativeMessageHandler())

            //4. pipeline of messagehandlers
            this.httpClient = new HttpClient(new Step1HttpMessageHandler(new Step2HttpMessageHandler(handler)))
            {
                //Url doesn't exist.
                BaseAddress = new System.Uri("http://url.doesnt.exist.com/api/")
            };
        }

        public async Task<T> Get<T>(string url) where T : class
        {
            try
            {
                var response = await httpClient.GetAsync(url);
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
