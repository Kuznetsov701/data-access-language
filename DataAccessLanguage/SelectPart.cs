using DataAccessLanguage.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public sealed class SelectPart : IExpressionPart
    {
        private IExpression expression;

        public ExpressionType Type => ExpressionType.Function;

        public SelectPart(IExpressionFactory expressionFactory, string parameter)
        {
            expression = expressionFactory.Create(parameter);
        }

        public object GetValue(object obj) =>
            obj switch
            {
                IEnumerable<object> list => list.Select(x => expression.GetValue(x)).ToList(),
                IDictionary map => map.Select(x => expression.GetValue(x)).ToList(),
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
                IEnumerable<object> list => (await list.SelectAsync(x => expression.GetValueAsync(x))).ToList(),
                IDictionary map => await map.SelectAsync(x => expression.GetValueAsync(x)),
                _ => null
            };

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}