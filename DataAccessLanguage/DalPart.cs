using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public sealed class DalPart : IExpressionPart
    {
        private IExpression expression;
        private readonly IExpressionFactory expressionFactory;

        public ExpressionType Type => ExpressionType.Function;

        public DalPart(IExpressionFactory expressionFactory, string parameter)
        {
            this.expression = expressionFactory.Create(parameter);
            this.expressionFactory = expressionFactory;
        }

        public object GetValue(object obj)
        {
            string strExpr = expression.GetValue(obj)?.ToString();
            if (strExpr == null)
                return null;
            IExpression expr = expressionFactory.Create(strExpr);
            return expr.GetValue(obj);
        }

        public bool SetValue(object obj, object value)
        {
            string strExpr = expression.GetValue(obj)?.ToString();
            if (strExpr == null)
                return false;
            IExpression expr = expressionFactory.Create(strExpr);
            return expr.SetValue(obj, value);
        }

        public async Task<object> GetValueAsync(object obj)
        {
            string strExpr = (await expression.GetValueAsync(obj))?.ToString();
            if (strExpr == null)
                return null;
            IExpression expr = expressionFactory.Create(strExpr);
            return await expr.GetValueAsync(obj);
        }

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}