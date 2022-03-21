using DataAccessLanguage.Demo.Blazor;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DataAccessLanguage.Demo
{
    internal class JsLazyLoad : IJsLazyLoad
    {
        private readonly IJSRuntime jsRuntime;

        Dictionary<string, Task> loadings = new Dictionary<string, Task>();

        public JsLazyLoad(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public Task LoadAsync(string url)
        {
            if (!loadings.ContainsKey(url))
                loadings.Add(url, Load(url));

            return loadings[url];
        }

        internal async Task Load(string url)
        {
            string code = File.ReadAllText(url);
            await jsRuntime.InvokeVoidAsync("eval", code);
        }
    }
}