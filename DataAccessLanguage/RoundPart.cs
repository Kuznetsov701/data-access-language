using System;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class RoundPart : IExpressionPart
    {
        private readonly int parameter;

        public ExpressionType Type => ExpressionType.Function;

        public RoundPart(string parameter)
        {
            this.parameter = int.TryParse(parameter, out int parsed)? parsed : 0;
        }

        public object GetValue(object dataObject) =>
            dataObject switch {
                float num => Math.Round(num, parameter),
                double num => Math.Round(num, parameter),
                decimal num => Math.Round(num, parameter),
                object num => decimal.TryParse(num?.ToString(), out decimal d)? Math.Round(d ,parameter) : null,
                _ => null
            };

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<object> GetValueAsync(object dataObject) =>
            Task.FromResult(GetValue(dataObject));

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}