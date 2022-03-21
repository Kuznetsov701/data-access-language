using DataAccessLanguage.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    class ParallelSelectManyPart : IExpressionPart
    {
        private IExpression expression;

        public ExpressionType Type => ExpressionType.Function;

        public ParallelSelectManyPart(IExpressionFactory expressionFactory, string parameter)
        {
            expression = expressionFactory.Create(parameter);
        }

        public object GetValue(object obj) =>
            obj switch
            {
                IEnumerable<object> list => list.SelectMany(x => expression.GetValue(x) as IEnumerable<object> ?? Enumerable.Empty<object>()).ToList(),
                _ => null
            };

        public bool SetValue(object obj, object value) =>
            obj switch
            {
                IEnumerable<object> list => list.Select(x => expression.SetValue(x, value)).ToList().Any(x => true),
                _ => false
            };

        public async Task<object> GetValueAsync(object obj) =>
            obj switch
            {
                IEnumerable<object> list => (await list.ParallelSelectAsync(x => expression.GetValueAsync(x))).SelectMany(x => x as IEnumerable<object> ?? Enumerable.Empty<object>()).ToList(),
                _ => null
            };

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}