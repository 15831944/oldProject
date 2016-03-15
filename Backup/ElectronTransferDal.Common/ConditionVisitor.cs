using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ElectronTransferDal.Common
{
    public class ConditionVisitor : ExpressionVisitor
    {
        private StringBuilder sb = new StringBuilder();
        bool Proccesed { get { return sb.Length > 0; } }
        Expression _expression;
        public ConditionVisitor(Expression expression)
        {
            _expression = expression;
        }
        public void Run()
        {
            if (!Proccesed)
                Visit(_expression);
        }
        private Expression VisitMethodCallIter(MethodCallExpression m)
        {
            Dictionary<string, string> stringCommands = new Dictionary<string, string>();
            stringCommands.Add("Contains", " LIKE '%{0}%'");
            stringCommands.Add("StartsWith", " LIKE '{0}%'");
            stringCommands.Add("EndsWith", " LIKE '%{0}'");
            var expr = m;

            var methodCall = expr as MethodCallExpression;
            var declareType = methodCall.Method.DeclaringType;
            var methodName = methodCall.Method.Name;
            if (declareType == typeof(string))
            {
                if (stringCommands.ContainsKey(methodName))
                {
                    sb.Append((methodCall.Object as MemberExpression).Member.Name);
                    object value = Expression.Lambda(methodCall.Arguments[0]).Compile().DynamicInvoke();
                    sb.AppendFormat(stringCommands[methodName], value);
                }
            }
            return m;
        }
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            AddValue(m);
            return m;
        }
        protected override Expression VisitBinary(BinaryExpression b)
        {
            if (b.NodeType == ExpressionType.And)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" AND ");
                Expression right = this.Visit(b.Right);
                return Expression.And(left, right);
            }
            else if (b.NodeType == ExpressionType.AndAlso)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" AND ");
                Expression right = this.Visit(b.Right);
                return Expression.AndAlso(left, right);
            }
            else if (b.NodeType == ExpressionType.GreaterThan)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" > ");
                Expression right = this.Visit(b.Right);
                return Expression.GreaterThan(left, right);
            }
            else if (b.NodeType == ExpressionType.GreaterThanOrEqual)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" >= ");
                Expression right = this.Visit(b.Right);
                return Expression.GreaterThanOrEqual(left, right);
            }
            else if (b.NodeType == ExpressionType.LessThan)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" < ");
                Expression right = this.Visit(b.Right);
                return Expression.LessThan(left, right);
            }
            else if (b.NodeType == ExpressionType.LessThanOrEqual)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" <= ");
                Expression right = this.Visit(b.Right);
                return Expression.LessThanOrEqual(left, right);
            }
            else if (b.NodeType == ExpressionType.Equal)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" = ");
                Expression right = this.Visit(b.Right);
                return Expression.Equal(left, right);
            }
            else if (b.NodeType == ExpressionType.Not)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" NOT ");
                Expression right = this.Visit(b.Right);
            }
            else if (b.NodeType == ExpressionType.NotEqual)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" != ");
                Expression right = this.Visit(b.Right);
                return Expression.NotEqual(left, right);
            }
            else if (b.NodeType == ExpressionType.Or)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" OR ");
                Expression right = this.Visit(b.Right);
                return Expression.Or(left, right);
            }
            else if (b.NodeType == ExpressionType.Modulo)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" MOD ");
                Expression right = this.Visit(b.Right);
                return Expression.Modulo(left, right);
            }
            else if (b.NodeType == ExpressionType.Add)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" + ");
                Expression right = this.Visit(b.Right);
                return Expression.Add(left, right);
            }
            else if (b.NodeType == ExpressionType.Subtract)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" - ");
                Expression right = this.Visit(b.Right);
                return Expression.Subtract(left, right);
            }
            else if (b.NodeType == ExpressionType.Multiply)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" * ");
                Expression right = this.Visit(b.Right);
                return Expression.Multiply(left, right);
            }
            else if (b.NodeType == ExpressionType.Divide)
            {
                Expression left = this.Visit(b.Left);
                sb.Append(" / ");
                Expression right = this.Visit(b.Right);
                return Expression.Divide(left, right);
            }
            return base.VisitBinary(b);
        }
        
        protected override Expression VisitConstant(ConstantExpression c)
        {
            AddValue(c);
            return base.VisitConstant(c);
        }

        private void AddValue(Expression c)
        {
            object value = Expression.Lambda(c).Compile().DynamicInvoke();
            if (value is string || value is char || value is DateTime)
                sb.AppendFormat("\'{0}\'", value);
            else
                sb.Append(value);
        }
        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression is ParameterExpression)
                sb.Append(m.Member.Name);
            else
            {
                AddValue(m);
            }
            return m;
        }
        protected override Expression VisitLambda(LambdaExpression lambda)
        {
            if (lambda.Parameters.Count == 1 && lambda.Parameters[0].NodeType == ExpressionType.Parameter && lambda.Body is MethodCallExpression)
            {
                return VisitMethodCallIter(lambda.Body as MethodCallExpression );
            }
            return base.VisitLambda(lambda);
            
        }

        public string BuildCondition()
        {
            if (sb.Length == 0)
                Run();
            return sb.ToString();
        }

        public override string ToString()
        {
            return BuildCondition();
        }
    }

}
