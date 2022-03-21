using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Types
{
    public class LongFunction : IExpressionPart
    {
        private long? value;
        private readonly IExpression expression;

        public LongFunction(IExpressionFactory expressionFactory, string value)
        {
            if (long.TryParse(value, out long v))
                this.value = v;
            else
                expression = expressionFactory.Create(value);
        }

        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            if (value is not null)
                return value;
            else if (long.TryParse(expression.GetValue(dataObject)?.ToString(), out long v))
                return v;
            else return null;
        }

        public async Task<object> GetValueAsync(object dataObject)
        {
            if (value is not null)
                return value;
            else if (long.TryParse((await expression.GetValueAsync(dataObject))?.ToString(), out long v))
                return v;
            else return null;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}