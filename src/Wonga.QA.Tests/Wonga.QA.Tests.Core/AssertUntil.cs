using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Core
{
    /// <summary>An alternative to Until.Do that causes an assert instead of a DoException.</summary>
    public static class AssertDo
    {
        public static dynamic Until<T>(Func<T> predicate, string assertMessage, params object[] messageArgs)
        {
            return Do.With.Until(predicate, assertMessage, messageArgs);
        }
    }
}
