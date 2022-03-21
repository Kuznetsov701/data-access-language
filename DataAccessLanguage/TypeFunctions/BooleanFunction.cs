using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Types
{
    public class BooleanFunction : IExpressionPart
    {
        private bool? value;
        private readonly IExpression expression;

        public BooleanFunction(IExpressionFactory expressionFactory, string value)
        {
            if (bool.TryParse(value, out bool v))
                this.value = v;
            else
                expression = expressionFactory.Create(value);
        }

        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            if (value is not null)
                return value;
            else if (bool.TryParse(expression.GetValue(dataObject)?.ToString(), out bool v))
                return v;
            else return null;
        }

        public async Task<object> GetValueAsync(object dataObject)
        {
            if (value is not null)
                return value;
            else if (bool.TryParse((await expression.GetValueAsync(dataObject))?.ToString(), out bool v))
                return v;
            else return null;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}