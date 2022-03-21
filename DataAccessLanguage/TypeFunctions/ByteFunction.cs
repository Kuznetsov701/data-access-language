using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Types
{
    public class ByteFunction : IExpressionPart
    {
        private byte? value;
        private readonly IExpression expression;

        public ByteFunction(IExpressionFactory expressionFactory, string value)
        {
            if (byte.TryParse(value, out byte v))
                this.value = v;
            else
                expression = expressionFactory.Create(value);
        }

        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            if (value is not null)
                return value;
            else if (byte.TryParse(expression.GetValue(dataObject)?.ToString(), out byte v))
                return v;
            else return null;
        }

        public async Task<object> GetValueAsync(object dataObject)
        {
            if (value is not null)
                return value;
            else if (byte.TryParse((await expression.GetValueAsync(dataObject))?.ToString(), out byte v))
                return v;
            else return null;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}