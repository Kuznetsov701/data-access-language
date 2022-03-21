using DataAccessLanguage.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    class InPart : IExpressionPart
    {
        private IExpression expression;

        public ExpressionType Type => ExpressionType.Function;

        public InPart(IExpressionFactory expressionFactory, string parameter)
        {
            expression = expressionFactory.Create(parameter);
        }

        public object GetValue(object obj) =>
            obj switch
            {
                object => (expression.GetValue(obj) as IEnumerable<object> ?? Enumerable.Empty<object>()).Any(x => x?.Equals(obj) == true),
                _ => null
            };

        public async Task<object> GetValueAsync(object obj) =>
            obj switch
            {
                object => (await expression.GetValueAsync(obj) as IEnumerable<object> ?? Enumerable.Empty<object>()).Any(x => x?.Equals(obj) == true),
                _ => null
            };

        public bool SetValue(object obj, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}