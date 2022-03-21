using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public sealed class AddPart : IExpressionPart
    {
        private List<IExpression> expressions = new();

        public ExpressionType Type => ExpressionType.Function;

        public AddPart(IExpressionFactory expressionFactory, string parameter)
        {
            Regex regex = new Regex(@"(?<expr>(((?!,.*)[^()&,]+)|(?<kek>\((?>[^()]+|\((?<depth>)|\)(?<-depth>))*(?(depth)(?!))\)))+)[\s]*[,]*[\s]*");

            var col = regex.Matches(parameter.Replace("\n.", ".").Replace("\n", " ").Replace("\r", " ").Replace("\t", " "));

            foreach (Match x in col)
                if (x.Groups["expr"].Success && !string.IsNullOrWhiteSpace(x.Groups["expr"].Value))
                    expressions.Add(expressionFactory.Create(x.Groups["expr"].Value));
        }

        private object Add(IList o)
        {
            foreach (var exp in expressions)
                o.Add(exp.GetValue(o));
            return o;
        }

        private object Add(IDictionary o)
        {
            for (int i = 0; i < expressions.Count; i += 2)
                o.Add(expressions[i].GetValue(o), i + 1 < expressions.Count ? expressions[i + 1].GetValue(o) : null);
            return o;
        }

        private object Add(IEnumerable<object> o)
        {
            List<object> list = o.ToList();
            Add(list as IList);
            return list;
        }


        private async Task<object> AddAsync(IList o)
        {
            foreach (var exp in expressions)
                o.Add(await exp.GetValueAsync(o));
            return o;
        }

        private async Task<object> AddAsync(IDictionary o)
        {
            for (int i = 0; i < expressions.Count; i += 2)
                o.Add(await expressions[i].GetValueAsync(o), i + 1 < expressions.Count ? await expressions[i + 1].GetValueAsync(o) : null);
            return o;
        }

        private async Task<object> AddAsync(IEnumerable<object> o)
        {
            List<object> list = o.ToList();
            await AddAsync(list as IList);
            return list;
        }

        public object GetValue(object obj) =>
            obj switch
            {
                IDictionary o => Add(o),
                IList o => Add(o),
                IEnumerable<object> o => Add(o),
                _ => null
            };

        public bool SetValue(object obj, object value) =>
            throw new NotImplementedException();

        public Task<object> GetValueAsync(object obj) =>
            obj switch
            {
                IDictionary o => AddAsync(o),
                IList o => AddAsync(o),
                IEnumerable<object> o => AddAsync(o),
                _ => null
            };

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}
