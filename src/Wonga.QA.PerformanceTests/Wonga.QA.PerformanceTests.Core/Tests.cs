using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.PerformanceTests.Core
{
    public class Tests
    {
        [Test, Owner(Owner.FrancisChelladurai)]
        public void ApiJourneyLoadTest()
        {
            var helper = new Helper();
            helper.StartTest();
            
        }
    }
}
