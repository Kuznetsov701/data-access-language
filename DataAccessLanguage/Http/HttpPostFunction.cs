using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccessLanguage.Http
{
    public class HttpPostFunction : IExpressionPart
    {
        private readonly IExpression bodyExpression;
        private readonly IExpression contentTypeExpression;

        public ExpressionType Type => ExpressionType.Function;

        public HttpPostFunction(string parameter, IExpressionFactory expressionFactory)
        {
            Regex regex = new Regex(@"(?<expr>(((?!,.*)[^()&,]+)|(?<kek>\((?>[^()]+|\((?<depth>)|\)(?<-depth>))*(?(depth)(?!))\)))+)[\s]*[,]*[\s]*");

            var col = regex.Matches(parameter.Replace("\n.", ".").Replace("\n", " ").Replace("\r", " ").Replace("\t", " "));

            if (col[0].Groups["expr"].Success && !string.IsNullOrWhiteSpace(col[0].Groups["expr"].Value))
                bodyExpression = expressionFactory.Create(col[0].Groups["expr"].Value);

            if (col[1].Groups["expr"].Success && !string.IsNullOrWhiteSpace(col[1].Groups["expr"].Value))
                contentTypeExpression = expressionFactory.Create(col[1].Groups["expr"].Value);
        }

        public object GetValue(object dataObject) =>
            GetValueAsync(dataObject).Result;

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public async Task<object> GetValueAsync(object dataObject)
        {
            HttpFunctionObject httpFunctionObject = dataObject as HttpFunctionObject;
            if (httpFunctionObject == null)
                return null;

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, httpFunctionObject.Url);

            foreach (var h in httpFunctionObject.Headers)
            {
                if (httpRequestMessage.Headers.Contains(h.Key))
                    httpRequestMessage.Headers.Remove(h.Key);
                if(h.Value is string)
                    httpRequestMessage.Headers.Add(h.Key, h.Value as string);
                else if(h.Value is IEnumerable<string>)
                    httpRequestMessage.Headers.Add(h.Key, h.Value as IEnumerable<string>);
            }

            object obj = await bodyExpression?.GetValueAsync(httpFunctionObject.DataObject);
            string mediaType = await contentTypeExpression?.GetValueAsync(httpFunctionObject.DataObject) as string;

            httpRequestMessage.Content = obj switch { 
                Stream stream => new StreamContent(stream),
                string str when !string.IsNullOrWhiteSpace(mediaType) => new StringContent(str, Encoding.UTF8, mediaType),
                string str => new StringContent(str),
                _ => null
            };

            HttpResponseMessage response = await httpFunctionObject.Http.SendAsync(httpRequestMessage);
            return await response.Content.ReadAsStringAsync();
        }

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}