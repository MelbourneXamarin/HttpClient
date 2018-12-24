using System.Threading.Tasks;

namespace HttpDemo
{
    public interface IApiService
    {
        Task<T> Get<T>(string url) where T : class;
        Task<T> Post<T>(string url, object data) where T : class;
    }
}