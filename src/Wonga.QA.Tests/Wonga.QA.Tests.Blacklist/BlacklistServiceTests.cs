﻿using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Blacklist
{
    [Parallelizable(TestScope.All)]
    public class BlacklistServiceTests
    {
        [Test]
        public void BlacklistServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.Blacklist.IsRunning());
        }
    }
}
