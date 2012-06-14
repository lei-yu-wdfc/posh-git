using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tools.NonParallelHunter
{
    public static class Extensions
    {
        public static IEnumerable<MemberInfo> GetNonParallelTests(this Type type, bool includePendingOrIgnoredOrExplicit, AUT? specificAUT)
        {
            return type.GetMethods().Where(m => !m.IsDefined(typeof(ParallelizableAttribute), false)
                                             && m.IsDefined(typeof(TestAttribute), false)
                                             && m.IsAppplicationSpecific(specificAUT)
                                             && !m.IsPending(includePendingOrIgnoredOrExplicit));
        }

        public static bool IsTestFixture(this Type t)
        {
            return t.IsDefined(typeof(TestFixtureAttribute), true)
                   || t.GetMethods().Any(x => x.IsDefined(typeof(TestAttribute), true));
        }

        public static IEnumerable<Type> GetTestFixtures(this Assembly assembly, bool includePendingOrIgnoredOrExplicit, AUT? specificAut)
        {
            return assembly.GetTypes().Where(t => t.IsTestFixture()
                && t.IsAppplicationSpecific(specificAut)
                && !t.IsAbstract
                && t.IsClass
                && !t.IsPending(includePendingOrIgnoredOrExplicit));
        }

        public static bool IsAppplicationSpecific(this MemberInfo type, AUT? specificAut)
        {
            var autAttribute = type.GetCustomAttributes(typeof(AUTAttribute), false).FirstOrDefault() as AUTAttribute;
            if (!specificAut.HasValue || autAttribute == null)
                return true;
            return autAttribute.AUTs.Contains(specificAut.Value);
        }

        public static bool IsPending(this MemberInfo type, bool includePendingOrIgnoredOrExplicit)
        {
            if (includePendingOrIgnoredOrExplicit)
                return false;

            return type.IsDefined(typeof(PendingAttribute), false)
                                                     || type.IsDefined(typeof(ExplicitAttribute), false)
                                                     || type.IsDefined(typeof(IgnoreAttribute), false);
        }

        public static bool IsPending(this Type type, bool includePendingOrIgnoredOrExplicit)
        {
            if (includePendingOrIgnoredOrExplicit)
                return false;

            return type.IsDefined(typeof(PendingAttribute), false)
                                                     || type.IsDefined(typeof(ExplicitAttribute), false)
                                                     || type.IsDefined(typeof(IgnoreAttribute), false);
        }

        public static bool IsAppplicationSpecific(this Type type, bool includePendingOrIgnoredOrExplicit, AUT? specificAut)
        {
            var autAttribute = type.GetCustomAttributes(typeof(AUTAttribute), false).FirstOrDefault() as AUTAttribute;
            if (!specificAut.HasValue || autAttribute == null)
                return true;
            return autAttribute.AUTs.Contains(specificAut.Value) && type.GetNonParallelTests(includePendingOrIgnoredOrExplicit, specificAut).Count() > 0;
        }

        public static IEnumerable<Type> GetParallelFixtures(this IEnumerable<Type> types, TestScope testScope)
        {
            return types.Where(t =>
            {
                ParallelizableAttribute attr =
                    t.GetCustomAttributes(typeof(ParallelizableAttribute), false).FirstOrDefault() as
                    ParallelizableAttribute;
                return t.IsDefined(typeof(ParallelizableAttribute), false)
                       && attr != null && attr.Scope == testScope;
            });
        }

        public static IEnumerable<Type> GetNonParallelFixtures(this IEnumerable<Type> types)
        {
            return types.Where(t => !t.IsDefined(typeof(ParallelizableAttribute), false));
        }
    }
}
