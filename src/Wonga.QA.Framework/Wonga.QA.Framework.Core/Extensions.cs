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

        public static IList<T> ForEach<T>(this IList<T> list, Action<T> action)
        {
            foreach (T t in list)
                action(t);
            return list;
        }

		public static string ToPaymentCardDate(this DateTime value)
		{
			return value.ToString("yyyy-MM");
		}
    }
}
