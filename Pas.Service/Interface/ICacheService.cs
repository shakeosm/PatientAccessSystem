
namespace Pas.Service.Interface
{
    public interface ICacheService
    {
        //TODO: implement async methods
        string GetCacheValue(string key);

        T GetCacheValue<T>(string key);

        void SetCacheValue(string key, string value);

        void SetCacheValue<T>(string key, T item);

        //Task<string> GetCacheValueAsync(string key);
        //Task SetCacheValueAsync(string key, string value);
    }
}
