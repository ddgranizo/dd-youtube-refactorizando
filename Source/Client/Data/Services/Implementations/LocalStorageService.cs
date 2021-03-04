using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Refactorizando.Shared.Utilities;

namespace Refactorizando.Client.Data.Services.Implementations{

    public class LocalStorageService : ILocalStorageService
    {
        private List<string> _contextCookies;

        public LocalStorageService(IJSRuntime jSRuntime)
        {
            _contextCookies = new List<string>();
            JSRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
        }

        public IJSRuntime JSRuntime { get; }

        public async ValueTask<string> GetFromLocalStorage(string key)
        {
            AddCookieToContext(key);
            var exists = await ExistsLocalStorage(key);
            if (exists)
            {
                var response = await JSRuntime.InvokeAsync<string>("localStorage.getItem", key);
                return response;
            }
            return null;
        }

        public ValueTask<T> GetObjectFromLocalStorage<T>(string key)
        {
            AddCookieToContext(key);
            return new ValueTask<T>(JsonSerializer.Deserialize<T>(GetFromLocalStorage(key).Result));
        }

        public ValueTask<object> RemoveLocalStorage(string key)
        {
            AddCookieToContext(key);
            return JSRuntime.InvokeAsync<object>("localStorage.removeItem", key);
        }

        public ValueTask<object> SetInLocalStorage(string key, string value)
        {
            AddCookieToContext(key);
            return JSRuntime.InvokeAsync<object>("localStorage.setItem", key, value);
        }

        public ValueTask<object> SetObjectInLocalStorage(string key, object value)
        {
            AddCookieToContext(key);
            return SetInLocalStorage(key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveAllCookies()
        {
            foreach (var item in _contextCookies)
            {
                await Task.FromResult(RemoveLocalStorage(item));
            }
        }

        private void AddCookieToContext(string key)
        {
            if (!_contextCookies.Contains(key))
            {
                _contextCookies.Add(key);
            }
        }

        public async Task<T> GetFromLocalStorage<T>(string key)
        {
            var data = await GetFromLocalStorage(key);
            return ParseUtility.ChangeType<T>(data);
        }

        public ValueTask<bool> ExistsLocalStorage(string key)
        {
            return JSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", key);
        }
    }
}