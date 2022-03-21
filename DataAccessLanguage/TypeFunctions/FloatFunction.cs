using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Types
{
    public class FloatFunction : IExpressionPart
    {
        private float? value;
        private readonly IExpression expression;

        public FloatFunction(IExpressionFactory expressionFactory, string value)
        {
            if (float.TryParse(value, out float v))
                this.value = v;
            else
                expression = expressionFactory.Create(value);
        }

        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            if (value is not null)
                return value;
            else if (float.TryParse(expression.GetValue(dataObject)?.ToString(), out float v))
                return v;
            else return null;
        }

        public async Task<object> GetValueAsync(object dataObject)
        {
            if (value is not null)
                return value;
            else if (float.TryParse((await expression.GetValueAsync(dataObject))?.ToString(), out float v))
                return v;
            else return null;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}