using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Types
{
    public class DoubleFunction : IExpressionPart
    {
        private double? value;
        private readonly IExpression expression;

        public DoubleFunction(IExpressionFactory expressionFactory, string value)
        {
            if (double.TryParse(value, out double v))
                this.value = v;
            else
                expression = expressionFactory.Create(value);
        }

        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            if (value is not null)
                return value;
            else if (double.TryParse(expression.GetValue(dataObject)?.ToString(), out double v))
                return v;
            else return null;
        }

        public async Task<object> GetValueAsync(object dataObject)
        {
            if (value is not null)
                return value;
            else if (double.TryParse((await expression.GetValueAsync(dataObject))?.ToString(), out double v))
                return v;
            else return null;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}