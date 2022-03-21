using System;
using System.Threading.Tasks;

namespace DataAccessLanguage.Types
{
    public class DateTimeFunction : IExpressionPart
    {
        private DateTime? value;
        private readonly IExpression expression;

        public DateTimeFunction(IExpressionFactory expressionFactory, string value)
        {
            if (DateTime.TryParse(value, out DateTime v))
                this.value = v;
            else
                expression = expressionFactory.Create(value);
        }

        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            if (value is not null)
                return value;
            else if (DateTime.TryParse(expression.GetValue(dataObject)?.ToString(), out DateTime v))
                return v;
            else return null;
        }

        public async Task<object> GetValueAsync(object dataObject)
        {
            if (value is not null)
                return value;
            else if (DateTime.TryParse((await expression.GetValueAsync(dataObject))?.ToString(), out DateTime v))
                return v;
            else return null;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}