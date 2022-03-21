using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccessLanguage.Http
{
    public class HttpHeaderFucntion : IExpressionPart
    {
        private readonly IExpression keyExpression;
        private readonly IExpression valueExpression;

        public ExpressionType Type => ExpressionType.Function;

        public HttpHeaderFucntion(string parameter, IExpressionFactory expressionFactory)
        {
            Regex regex = new Regex(@"(?<expr>(((?!,.*)[^()&,]+)|(?<kek>\((?>[^()]+|\((?<depth>)|\)(?<-depth>))*(?(depth)(?!))\)))+)[\s]*[,]*[\s]*");

            var col = regex.Matches(parameter.Replace("\n.", ".").Replace("\n", " ").Replace("\r", " ").Replace("\t", " "));

            if (col[0].Groups["expr"].Success && !string.IsNullOrWhiteSpace(col[0].Groups["expr"].Value))
                keyExpression = expressionFactory.Create(col[0].Groups["expr"].Value);

            if (col[1].Groups["expr"].Success && !string.IsNullOrWhiteSpace(col[1].Groups["expr"].Value))
                valueExpression = expressionFactory.Create(col[1].Groups["expr"].Value);
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

            string key = await keyExpression?.GetValueAsync(httpFunctionObject.DataObject) as string;
            object value = await valueExpression?.GetValueAsync(httpFunctionObject.DataObject);

            httpFunctionObject.Headers.Add(KeyValuePair.Create(key, value));

            return dataObject;
        }

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}