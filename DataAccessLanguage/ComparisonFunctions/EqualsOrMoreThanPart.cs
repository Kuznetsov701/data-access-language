using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class EqualsOrMoreThanPart : IExpressionPart
    {
        private static int[] trueValue = { 1, 0 };
        private readonly IExpression parameterExpression;

        public ExpressionType Type => ExpressionType.Function;

        public EqualsOrMoreThanPart(IExpressionFactory expressionFactory, string parameter)
        {
            this.parameterExpression = expressionFactory.Create(parameter);
        }

        public object GetValue(object obj)
        {
            object parameter = parameterExpression.GetValue(obj);
            return GetResult(obj, parameter);
        }

        public bool SetValue(object obj, object value) =>
            throw new NotImplementedException();

        public async Task<object> GetValueAsync(object obj)
        {
            object parameter = await parameterExpression.GetValueAsync(obj);
            return GetResult(obj, parameter);
        }

        public bool? GetResult(object obj, object parameter) =>
           (obj, parameter) switch
           {
               (IComparable a, IComparable b) when a.GetType() == b.GetType() => trueValue.Contains(a.CompareTo(b)),
               (not null, not null) when decimal.TryParse(obj.ToString(), out decimal a) && decimal.TryParse(parameter.ToString(), out decimal b) => a >= b,
               (not null, not null) when DateTime.TryParse(obj.ToString(), out DateTime a) && DateTime.TryParse(parameter.ToString(), out DateTime b) => a >= b,
               (not null, not null) when TimeSpan.TryParse(obj.ToString(), out TimeSpan a) && TimeSpan.TryParse(parameter.ToString(), out TimeSpan b) => a >= b,
               _ => null
           };

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}
