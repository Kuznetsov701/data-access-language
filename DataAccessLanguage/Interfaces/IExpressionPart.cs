using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public interface IExpressionPart
    {
        ExpressionType Type { get; }

        Task<object> GetValueAsync(object dataObject);
        Task<bool> SetValueAsync(object dataObject, object value);
        object GetValue(object dataObject);
        bool SetValue(object dataObject, object value);
    }
}