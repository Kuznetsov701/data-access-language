using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Types
{
    public class TimeSpanFunction : IExpressionPart
    {
        private TimeSpan? value;
        private readonly IExpression expression;

        public TimeSpanFunction(IExpressionFactory expressionFactory, string value)
        {
            if (TimeSpan.TryParse(value, out TimeSpan v))
                this.value = v;
            else
                expression = expressionFactory.Create(value);
        }

        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            if (value is not null)
                return value;
            else if (TimeSpan.TryParse(expression.GetValue(dataObject)?.ToString(), out TimeSpan v))
                return v;
            else return null;
        }

        public async Task<object> GetValueAsync(object dataObject)
        {
            if (value is not null)
                return value;
            else if (TimeSpan.TryParse((await expression.GetValueAsync(dataObject))?.ToString(), out TimeSpan v))
                return v;
            else return null;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}