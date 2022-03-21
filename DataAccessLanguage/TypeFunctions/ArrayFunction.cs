using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public sealed class ArrayFunction : IExpressionPart
    {
        private List<IExpression> expressions = new List<IExpression>();

        public ExpressionType Type => ExpressionType.Function;

        public ArrayFunction(IExpressionFactory expressionFactory, string parameter)
        {
            Regex regex = new Regex(@"(?<expr>(((?!,.*)[^(),]+)|(?<kek>\((?>[^()]+|\((?<depth>)|\)(?<-depth>))*(?(depth)(?!))\)))+)");

            var col = regex.Matches(parameter.Replace("\n.", ".").Replace("\n", " ").Replace("\r", " ").Replace("\t", " "));

            foreach (Match x in col)
                if (x.Groups["expr"].Success && !string.IsNullOrWhiteSpace(x.Groups["expr"].Value))
                    expressions.Add(expressionFactory.Create(x.Groups["expr"].Value));
        }

        public object GetValue(object obj) =>
            obj switch
            {
                object o => CreateArray(o),
                _ => CreateArray(null)
            };

        private object CreateArray(object o)
        {
            List<object> result = new();
            foreach (var expr in expressions)
                result.Add(expr.GetValue(o));
            return result;
        }

        public bool SetValue(object obj, object value) =>
            throw new NotImplementedException();

        public async Task<object> GetValueAsync(object dataObject)
        {
            List<object> result = new();
            foreach (var expr in expressions)
                result.Add(await expr.GetValueAsync(dataObject));
            return result;
        }

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}