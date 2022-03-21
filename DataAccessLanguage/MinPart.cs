using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class MinPart : IExpressionPart
    {
        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject) =>
            dataObject switch {
                IEnumerable<int> list when list.Any() => list.Min(),
                IEnumerable<long> list when list.Any() => list.Min(),
                IEnumerable<float> list when list.Any() => list.Min(),
                IEnumerable<double> list when list.Any() => list.Min(),
                IEnumerable<decimal> list when list.Any() => list.Min(),
                IEnumerable<int?> list when list.Any() => list.Min(),
                IEnumerable<long?> list when list.Any() => list.Min(),
                IEnumerable<float?> list when list.Any() => list.Min(),
                IEnumerable<double?> list when list.Any() => list.Min(),
                IEnumerable<decimal?> list when list.Any() => list.Min(),
                IEnumerable<object> list when list.Any() => list.Select(x => { double.TryParse(x?.ToString(), out double d); return d; }).Min(),
                _ => null
            };

        public Task<object> GetValueAsync(object dataObject) =>
            Task.FromResult(GetValue(dataObject));

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}