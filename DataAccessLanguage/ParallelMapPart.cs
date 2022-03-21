using DataAccessLanguage.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public sealed class ParallelMapPart : IExpressionPart
    {
        private List<KeyValuePair<string, IExpression>> expressions = new();

        public ExpressionType Type => ExpressionType.Function;

        public ParallelMapPart(IExpressionFactory expressionFactory, string parameter)
        {
            Regex regex = new Regex(@"(?<expr>(((?!=>.*)[^()&=>]+)|(?<kek>\((?>[^()]+|\((?<depth>)|\)(?<-depth>))*(?(depth)(?!))\)))+)([\s]*=>[\s]*(?'name'[\w\d]+))*&{0,2}");

            var col = regex.Matches(parameter.Replace("\n.", ".").Replace("\n", " ").Replace("\r", " ").Replace("\t", " "));

            int nonameCount = 0;
            foreach (Match x in col)
            {
                if (x.Groups["expr"].Success && !string.IsNullOrWhiteSpace(x.Groups["expr"].Value))
                {
                    string name = x.Groups["name"].Value;
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        name = "noname" + ((nonameCount == 0) ? "" : "_" + nonameCount.ToString());
                        nonameCount++;
                    }
                    expressions.Add(KeyValuePair.Create(name, expressionFactory.Create(x.Groups["expr"].Value)));
                }
            }
        }

        public object GetValue(object obj) =>
            obj switch
            {
                object o => Map(o),
                _ => null
            };

        private object Map(object o)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            foreach (var expr in expressions)
                res.TryAdd(expr.Key, expr.Value.GetValue(o));
            return res;
        }

        private async Task<object> MapAsync(object o)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();

            var tasks =(await expressions.ParallelSelectAsync(async x => KeyValuePair.Create(x.Key, await x.Value.GetValueAsync(o)))).ToList();

            foreach (var expr in tasks)
                res.TryAdd(expr.Key, expr.Value);
            
            return res;
        }

        public bool SetValue(object obj, object value) =>
            throw new NotImplementedException();

        public Task<object> GetValueAsync(object obj) =>
            obj switch
            {
                object o => MapAsync(o),
                _ => null
            };

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}
