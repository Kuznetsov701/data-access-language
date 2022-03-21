using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Types
{
    public class DecimalFunction : IExpressionPart
    {
        private decimal? value;
        private readonly IExpression expression;

        public DecimalFunction(IExpressionFactory expressionFactory, string value)
        {
            if (decimal.TryParse(value, out decimal v))
                this.value = v;
            else
                expression = expressionFactory.Create(value);
        }

        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            if (value is not null)
                return value;
            else if (decimal.TryParse(expression.GetValue(dataObject)?.ToString(), out decimal v))
                return v;
            else return null;
        }

        public async Task<object> GetValueAsync(object dataObject)
        {
            if (value is not null)
                return value;
            else if (decimal.TryParse((await expression.GetValueAsync(dataObject))?.ToString(), out decimal v))
                return v;
            else return null;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}