using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public sealed class ToJsonPart : IExpressionPart
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public ExpressionType Type => ExpressionType.Function;

        public ToJsonPart(JsonSerializerOptions jsonSerializerOptions)
        {
            this.jsonSerializerOptions = jsonSerializerOptions;
        }

        public object GetValue(object dataObject) => JsonSerializer.Serialize(dataObject, jsonSerializerOptions);

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<object> GetValueAsync(object dataObject) =>
            Task.FromResult(GetValue(dataObject));

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}