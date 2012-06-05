using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    class SurveyTest : UiTest
    {
        [Test, AUT(AUT.Ca), JIRA("CA-1826")]
        public void IsSurveyAvailableOnHomePage()
        {
            var page = Client.Home();
            Assert.IsNotNull(page.Survey);
        }

        [Test, AUT(AUT.Ca),JIRA("CA-1826")]
        public void IsSurveyHiddenByDefaultOnHomePage()
        {
            var page = Client.Home();
            Assert.IsFalse(page.Survey.IsVisible);
        }

        [Test, AUT(AUT.Ca),JIRA("CA-1826")]
        public void IsSurveyVisibleAfter15SecondsOnHomePage()
        {
            var page = Client.Home();
            Thread.Sleep(18000);
            Assert.IsTrue(page.Survey.IsVisible);
        }

    
    }
}
