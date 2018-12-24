using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpDemo.HttpMessageHandlers
{
    /// <summary>
    /// Standard http client handler.
    /// Nothing Special
    /// </summary>
    public class StandardHttpClientHandler : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }
    }
}
