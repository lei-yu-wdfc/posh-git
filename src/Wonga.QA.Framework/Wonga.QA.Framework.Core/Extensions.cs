using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Wonga.QA.Framework.Core
{
    public static partial class Extensions
    {
        public static T GetAttribute<T>(this MemberInfo member) where T : Attribute
        {
            return (T)member.GetCustomAttributes(typeof(T), false).SingleOrDefault();
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T t in enumerable)
                action(t);
            return enumerable;
        }
    }
}
