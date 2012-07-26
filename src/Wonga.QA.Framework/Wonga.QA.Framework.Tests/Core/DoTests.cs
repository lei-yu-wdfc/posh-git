using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Tests.Core
{
    [TestFixture]
    class DoTests
    {
        [Test]
        public void DoesNotAddTwoExceptionsOfTheSameTypeOneAfterAnother()
        {
            var ex = Assert.Throws<DoException>(() => Do.With.Interval(1).Timeout(TimeSpan.FromSeconds(3)).Until(() =>
                         {
                             throw new Exception();
                             return 0;
                         }));
            Assert.AreEqual(2, ex.Exceptions.Count);
        }

        [Test]
        public void CanAddTwoExceptionsOfDifferentTypeOneAfterAnother()
        {
            var i = 0;
            var ex = Assert.Throws<DoException>(() => Do.With.Interval(1).Timeout(TimeSpan.FromSeconds(4)).Until(() =>
            {
                if (i == 0)
                {
                    i++;
                    throw new Exception();
                }
                throw new ExecutionEngineException();
                return 0;
            }));
            Assert.AreEqual(3, ex.Exceptions.Count);
        }
    }
}
