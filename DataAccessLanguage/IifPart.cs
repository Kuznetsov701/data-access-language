using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class IifPart : IExpressionPart
    {
        private IExpression condition;
        private IExpression trueValue;
        private IExpression falseValue;

        public ExpressionType Type => ExpressionType.Function;

        public IifPart(IExpressionFactory expressionFactory, string parameter)
        {
            Regex regex = new Regex(@"(?<expr>([^(),]+|(\((?>[^()]+|\((?<depth>)|\)(?<-depth>))*(?(depth)(?!))\)))+)");
            MatchCollection matchCollection = regex.Matches(parameter);

            if (matchCollection.Count >= 1 && matchCollection[0].Groups["expr"].Success && !string.IsNullOrWhiteSpace(matchCollection[0].Groups["expr"].Value))
                condition = expressionFactory.Create(matchCollection[0].Groups["expr"].Value);

            if (matchCollection.Count >= 2 && matchCollection[1].Groups["expr"].Success && !string.IsNullOrWhiteSpace(matchCollection[1].Groups["expr"].Value))
                trueValue = expressionFactory.Create(matchCollection[1].Groups["expr"].Value);

            if (matchCollection.Count >= 3 && matchCollection[2].Groups["expr"].Success && !string.IsNullOrWhiteSpace(matchCollection[2].Groups["expr"].Value))
                falseValue = expressionFactory.Create(matchCollection[2].Groups["expr"].Value);
        }

        public object GetValue(object dataObject)
        {
            if (condition == null)
                return null;
            if ((condition?.GetValue(dataObject) as bool?) == true)
                return trueValue != null ? trueValue?.GetValue(dataObject) : null;
            else
                return falseValue != null ? falseValue?.GetValue(dataObject) : null;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public async Task<object> GetValueAsync(object dataObject)
        {
            if (condition == null)
                return null;
            if (((await condition?.GetValueAsync(dataObject)) as bool?) == true)
                return trueValue != null ? await trueValue?.GetValueAsync(dataObject) : null;
            else
                return falseValue != null ? await falseValue?.GetValueAsync(dataObject) : null;
        }

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}