using System.Threading.Tasks;

namespace Refactorizando.Client.Data.Services {

    public interface ILocalStorageService
    {
        ValueTask<object> SetInLocalStorage(string key, string value);
        ValueTask<object> SetObjectInLocalStorage(string key, object value);
        ValueTask<string> GetFromLocalStorage(string key);
        Task<T> GetFromLocalStorage<T>(string key);
        ValueTask<T> GetObjectFromLocalStorage<T>(string key);
        ValueTask<object> RemoveLocalStorage(string key);
        Task RemoveAllCookies();

        ValueTask<bool> ExistsLocalStorage(string key);
    }
}