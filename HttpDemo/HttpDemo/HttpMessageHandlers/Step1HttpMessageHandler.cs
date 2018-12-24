using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpDemo.HttpMessageHandlers
{
    /// <summary>
    /// Step1 http message handler.
    /// Checks the request to determine if we're expecting a single item
    /// Wraps the response content object into an array
    /// </summary>
    public class Step1HttpMessageHandler : DelegatingHandler
    {
        public Step1HttpMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Step 1: {request.Method}");
            var response = await base.SendAsync(request, cancellationToken);

            if (int.TryParse(request.RequestUri.Segments.Last(), out int result))
            {
                //Looking for a single object...
                //Lets put that in a list just cause
                var responseContent = await response.Content.ReadAsStringAsync();
                var json = $"[ {responseContent} ]";
                var test = new HttpResponseMessage() { Content = new StringContent(json, Encoding.UTF8, "application/json") };
                return test;
            }
            return response;
        }
    }
}
