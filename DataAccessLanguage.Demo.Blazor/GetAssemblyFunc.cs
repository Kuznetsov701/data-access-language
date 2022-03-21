using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Demo.Blazor
{
    public class GetAssemblyFunc : IExpressionPart
    {
        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject) => typeof(Program).Assembly.GetName().Name;

        public Task<object> GetValueAsync(object dataObject) => Task.FromResult(GetValue(dataObject));

        public bool SetValue(object dataObject, object value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetValueAsync(object dataObject, object value)
        {
            throw new NotImplementedException();
        }
    }
}
