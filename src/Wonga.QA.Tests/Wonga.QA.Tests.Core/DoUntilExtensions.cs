using MbUnit.Framework;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Core
{
    /// <summary>Wraps Do.Until with try/catch and emit an Assert Failure if a DoException occurs.</summary>
    public static class DoUntilExtensions
    {
        public static T Until<T>(this DoBuilder builder, System.Func<T> predicate, string assertMessage, params object[] messageArgs)
        {
            T result = default(T);
            try
            {
                result = builder.Until(predicate);
            }
            catch (DoException)
            {
                Assert.Fail(assertMessage, messageArgs);
            }
            return result;
        }
    }
}