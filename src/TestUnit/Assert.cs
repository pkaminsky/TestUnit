using System;
using System.Linq.Expressions;

namespace TestUnit {
    public static class Assert {

        public static void True(bool value, string errorMessage = null) {
            if (!value) {
                throw new AssertException(errorMessage ?? "The test evaluated to false; expected true");
            }
        }

        public static void False(bool value, string errorMessage = null) {
            if (value) {
                throw new AssertException(errorMessage ?? "The test evaluated to false; expected true");
            }
        }

        public static void Throws<TException>(Expression<Func<bool>> expr) {
            try {
                True(expr);
            }
            catch (Exception ex) when (ex is TException) {

            }

            throw new AssertException($"Did not throw expected exception of type {typeof(TException).Name}");
        }

        public static void True(Expression<Func<bool>> expression) {
            Expression bodyExp = expression.Body;            
            
            switch (bodyExp.NodeType) {

                /* i'm pretty sure these can be done, and can be handled cutely */
                case ExpressionType.MemberAccess:
                    AssertMemberTrue(expression);
                    break;
                case ExpressionType.Equal:
                    AssertEquals(expression);
                    break;

                case ExpressionType.Block:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Call:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Invoke:
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.Lambda:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Not:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.TypeAs:
                case ExpressionType.TypeEqual:
                case ExpressionType.TypeIs:
                    AssertTrueExpr(expression);
                    break;


                /* i don't expect to do anything cute with most of these, either because they're impossible or too niche */
                case ExpressionType.Add:
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AddChecked:
                case ExpressionType.AndAssign:
                case ExpressionType.ArrayIndex:
                case ExpressionType.ArrayLength:
                case ExpressionType.Assign: //maybe
                case ExpressionType.Coalesce:
                case ExpressionType.Conditional: //maybe
                case ExpressionType.Constant: //technically
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.DebugInfo:
                case ExpressionType.Decrement:
                case ExpressionType.Default:
                case ExpressionType.Divide:
                case ExpressionType.DivideAssign:
                case ExpressionType.Dynamic:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.Extension:
                case ExpressionType.Goto:
                case ExpressionType.Increment:
                case ExpressionType.Index:
                case ExpressionType.Label:
                case ExpressionType.LeftShift:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ListInit:
                case ExpressionType.Loop:
                case ExpressionType.MemberInit:
                case ExpressionType.Modulo:
                case ExpressionType.ModuloAssign:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.New:
                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                case ExpressionType.OnesComplement:
                case ExpressionType.OrAssign:
                case ExpressionType.Parameter:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.Power:
                case ExpressionType.PowerAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.Quote: //inception?
                case ExpressionType.RightShift:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.RuntimeVariables:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Switch:
                case ExpressionType.Throw:
                case ExpressionType.Try: //i think try has to be in a block
                case ExpressionType.UnaryPlus:
                case ExpressionType.Unbox: //i dont think it can be expressed
                    throw new InvalidOperationException($"Can't parse expression--must be boolean function. {bodyExp.ToString()}");

            }
        }

        internal static void AssertTrueExpr(Expression<Func<bool>> expression) {
            var result = expression.Compile().Invoke();
            if (!result) {
                throw new AssertException($"{expression.Body.ToString()} | did not evaluate as 'true'");
            }
        }

        internal static void AssertMemberTrue(Expression<Func<bool>> expression) {
            var result = expression.Compile().Invoke();
            if (!result) {
                var mex = expression.Body as MemberExpression;
                throw new AssertException($"Member '{mex.Member.Name}' from '{mex.Member.DeclaringType}' was false");
            }
        }

        internal static void AssertEquals(Expression<Func<bool>> expression) {
            //todo: be fancy? 
            //else
            AssertTrueExpr(expression);
        }
    }
}