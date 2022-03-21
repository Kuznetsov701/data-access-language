using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLanguage.Http
{
    public class HttpGetFunction : IExpressionPart
    {
        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject) => 
            GetValueAsync(dataObject).Result;

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public async Task<object> GetValueAsync(object dataObject)
        {
            HttpFunctionObject httpFunctionObject = dataObject as HttpFunctionObject;
            if (httpFunctionObject == null)
                return null;

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, httpFunctionObject.Url);

            foreach (var h in httpFunctionObject.Headers)
            {
                if (httpRequestMessage.Headers.Contains(h.Key))
                    httpRequestMessage.Headers.Remove(h.Key);
                if (h.Value is string)
                    httpRequestMessage.Headers.Add(h.Key, h.Value as string);
                else if (h.Value is IEnumerable<string>)
                    httpRequestMessage.Headers.Add(h.Key, h.Value as IEnumerable<string>);
            }

            var response = await httpFunctionObject.Http.SendAsync(httpRequestMessage);
            return await response.Content.ReadAsStringAsync();
        }

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}