using System;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class EqualsPart : IExpressionPart
    {
        private IExpression parameterExpression;
        public ExpressionType Type => ExpressionType.Function;

        public EqualsPart(IExpressionFactory expressionFactory, string parameter)
        {
            this.parameterExpression = expressionFactory.Create(parameter);
        }

        public object GetValue(object obj)
        {
            object parameter = parameterExpression.GetValue(obj);

            return obj switch
            {
                null when parameter is null => true,
                object o when o is not null && o.Equals(parameter) => true,
                object o when o is not null && o?.ToString()?.Equals(parameter?.ToString()) == true => true,
                _ => false
            };
        }

        public bool SetValue(object obj, object value) =>
            throw new NotImplementedException();

        public async Task<object> GetValueAsync(object obj)
        {
            object parameter = await parameterExpression.GetValueAsync(obj);

            return obj switch
            {
                null when parameter is null => true,
                object o when o is not null && o.Equals(parameter) => true,
                object o when o is not null && o?.ToString()?.Equals(parameter?.ToString()) == true => true,
                _ => false
            };
        }

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}