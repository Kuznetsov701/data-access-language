using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class MaxPart : IExpressionPart
    {
        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            return dataObject switch
            {
                IEnumerable<int> list when list.Any() => list.Max(),
                IEnumerable<long> list when list.Any() => list.Max(),
                IEnumerable<float> list when list.Any() => list.Max(),
                IEnumerable<double> list when list.Any() => list.Max(),
                IEnumerable<decimal> list when list.Any() => list.Max(),
                IEnumerable<int?> list when list.Any() => list.Max(),
                IEnumerable<long?> list when list.Any() => list.Max(),
                IEnumerable<float?> list when list.Any() => list.Max(),
                IEnumerable<double?> list when list.Any() => list.Max(),
                IEnumerable<decimal?> list when list.Any() => list.Max(),
                IEnumerable<object> list when list.Any() => list.Select(x => { decimal.TryParse(x?.ToString(), out decimal d); return d; }).Max(),
                _ => null
            };
        }

        public Task<object> GetValueAsync(object dataObject) =>
            Task.FromResult(GetValue(dataObject));

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}