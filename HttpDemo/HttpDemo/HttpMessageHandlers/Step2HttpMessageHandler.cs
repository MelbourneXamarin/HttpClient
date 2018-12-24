using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpDemo.HttpMessageHandlers
{
    /// <summary>
    /// Step2 http message handler.
    /// Nothing special. Just showing MessageHandler Pipelines
    /// Writes the request method to the console
    /// </summary>
    public class Step2HttpMessageHandler : DelegatingHandler
    {
        public Step2HttpMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Step 2: {request.Method}");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
