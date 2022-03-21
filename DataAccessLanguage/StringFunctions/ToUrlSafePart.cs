using System;
using System.Threading.Tasks;
using System.Web;

namespace DataAccessLanguage
{
    public sealed class ToUrlSafePart : IExpressionPart
    {
        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject) =>
            dataObject switch {
                string s => HttpUtility.UrlEncode(s),
                _ => null
            };

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<object> GetValueAsync(object dataObject) =>
            Task.FromResult(GetValue(dataObject));

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}