using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Types
{
    public class NullPart : IExpressionPart
    {
        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject) => null;

        public Task<object> GetValueAsync(object dataObject) => Task.FromResult(GetValue(dataObject));

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}