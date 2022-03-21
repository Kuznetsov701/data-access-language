using System;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class NotEqualsPart : IExpressionPart
    {
        private readonly IExpression parameterExpression;
        public ExpressionType Type => ExpressionType.Function;

        public NotEqualsPart(IExpressionFactory expressionFactory, string parameter)
        {
            this.parameterExpression = expressionFactory.Create(parameter);
        }

        public object GetValue(object obj)
        {
            object parameter = parameterExpression.GetValue(obj);

            return obj switch
            {
                null when parameter is null => false,
                object o when o is not null && o.Equals(parameter) => false,
                object o when o is not null && o?.ToString()?.Equals(parameter?.ToString()) == true => false,
                _ => true
            };
        }

        public bool SetValue(object obj, object value) =>
            throw new NotImplementedException();

        public async Task<object> GetValueAsync(object obj)
        {
            object parameter = await parameterExpression.GetValueAsync(obj);

            return obj switch
            {
                null when parameter is null => false,
                object o when o is not null && o.Equals(parameter) => false,
                object o when o is not null && o?.ToString()?.Equals(parameter?.ToString()) == true => false,
                _ => true
            };
        }

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}