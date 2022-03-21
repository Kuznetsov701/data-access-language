using DataAccessLanguage.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class DistinctPart : IExpressionPart
    {
        public ExpressionType Type => ExpressionType.Function;

        private IExpression expression;

        public DistinctPart(IExpressionFactory expressionFactory, string parameter)
        {
            expression = expressionFactory.Create(parameter);
        }

        public object GetValue(object dataObject) =>
            dataObject switch
            {
                IEnumerable<object> e => e.Select(x => expression.GetValue(x)).Distinct().ToList(),
                _ => null
            };

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public async Task<object> GetValueAsync(object dataObject) =>
            dataObject switch
            {
                IEnumerable<object> e => (await e.SelectAsync(x => expression.GetValueAsync(x))).Distinct().ToList(),
                _ => null
            };

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}
