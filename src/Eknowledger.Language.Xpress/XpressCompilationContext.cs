using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Eknowledger.Language.Xpress
{
    internal class XpressCompilationContext
    {
        internal static MethodInfo RuntimeContextGet = typeof(XpressRuntimeContext).GetMethod("Get",
                (BindingFlags.Public | BindingFlags.Instance), null, CallingConventions.Any, new Type[] { typeof(string) }, null);

        internal static MethodInfo RuntimeContextExists = typeof(XpressRuntimeContext).GetMethod("Exists",
                (BindingFlags.Public | BindingFlags.Instance), null, CallingConventions.Any, new Type[] { typeof(string) }, null);

        internal static MethodInfo IsNullOrEmptyMethod = typeof(string).GetMethod("IsNullOrEmpty",
                    (BindingFlags.Public | BindingFlags.Static), null, CallingConventions.Any, new Type[] { typeof(string) }, null);

        internal static MethodInfo StringEquals = typeof(string).GetMethod("Equals",
                    (BindingFlags.Public | BindingFlags.Static), null, CallingConventions.Any, new Type[] { typeof(string), typeof(string), typeof(StringComparison) }, null);

        internal Expression RuntimeContextParameter;
    }
}
