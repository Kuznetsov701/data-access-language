using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Types
{
    public class IntegerFunction : IExpressionPart
    {
        private int? value;
        private readonly IExpression expression;

        public IntegerFunction(IExpressionFactory expressionFactory, string value)
        {
            if (int.TryParse(value, out int v))
                this.value = v;
            else
                expression = expressionFactory.Create(value);
        }

        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            if (value is not null)
                return value;
            else if (int.TryParse(expression.GetValue(dataObject)?.ToString(), out int v))
                return v;
            else return null;
        }

        public async Task<object> GetValueAsync(object dataObject)
        {
            if (value is not null)
                return value;
            else if (int.TryParse((await expression.GetValueAsync(dataObject))?.ToString(), out int v))
                return v;
            else return null;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}