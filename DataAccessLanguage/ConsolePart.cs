﻿using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class ConsolePart : IExpressionPart
    {
        public ExpressionType Type => ExpressionType.Function;

        public object GetValue(object dataObject)
        {
            Console.WriteLine(JsonSerializer.Serialize(dataObject));
            return dataObject;
        }

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public Task<object> GetValueAsync(object dataObject) =>
            Task.FromResult(GetValue(dataObject));

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}