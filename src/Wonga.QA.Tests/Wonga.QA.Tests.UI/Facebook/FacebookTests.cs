using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Facebook
{
    [TestFixture, Parallelizable(TestScope.All)]
    class FacebookTests : UiTest
    {

        [Test, AUT(AUT.Uk), JIRA("UKFB-6")]
        public void VerifyFacebookThemeIsLoaded()
        {
            var homePage = Client.Home();
            Assert.IsTrue(Regex.IsMatch(homePage.Source, "<body class=\".*in-facebook.*\">", RegexOptions.IgnoreCase));
        }

        [Test, AUT(AUT.Uk), JIRA("UKFB-6")]
        public void VerifyFacebookSdkIsIncluded()
        {
            var homePage = Client.Home();
            Assert.IsTrue(Regex.IsMatch(homePage.Source, "connect.facebook.net/en_US/all.js", RegexOptions.IgnoreCase));
        }

    }
}