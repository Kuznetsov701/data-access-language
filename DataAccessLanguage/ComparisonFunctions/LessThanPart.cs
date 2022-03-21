using System;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class LessThanPart : IExpressionPart
    {
        IExpression parameterExpression;

        public ExpressionType Type => ExpressionType.Function;

        public LessThanPart(IExpressionFactory expressionFactory, string parameter)
        {
            this.parameterExpression = expressionFactory.Create(parameter);
        }

        public object GetValue(object obj)
        {
            string parameter = parameterExpression.GetValue(obj)?.ToString();
            return GetResult(obj, parameter);
        }

        public bool SetValue(object obj, object value) =>
            throw new NotImplementedException();

        public async Task<object> GetValueAsync(object obj)
        {
            string parameter = (await parameterExpression.GetValueAsync(obj))?.ToString();
            return GetResult(obj, parameter);
        }

        public bool? GetResult(object obj, string parameter) =>
            obj switch
            {
                not null when decimal.TryParse(obj.ToString(), out decimal a) && decimal.TryParse(parameter, out decimal b) => a < b,
                DateTime a when DateTime.TryParse(parameter, out DateTime b) => a < b,
                TimeSpan a when TimeSpan.TryParse(parameter, out TimeSpan b) => a < b,
                not null when DateTime.TryParse(obj.ToString(), out DateTime a) && DateTime.TryParse(parameter, out DateTime b) => a < b,
                not null when TimeSpan.TryParse(obj.ToString(), out TimeSpan a) && TimeSpan.TryParse(parameter, out TimeSpan b) => a < b,
                _ => null
            };

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}