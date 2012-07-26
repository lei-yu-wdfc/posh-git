using System.Text.RegularExpressions;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Facebook
{
    [TestFixture, Parallelizable(TestScope.All)]
    class FacebookTests : UiTest
    {

        [Test, AUT(AUT.Uk), JIRA("UKFB-6"), Pending("Waiting on Facebook Environment Setup - AUT.UkFb")]
        public void VerifyFacebookThemeIsLoaded()
        {
            var homePage = Client.Home();
            Assert.IsTrue(Regex.IsMatch(homePage.Source, "<body class=\".*in-facebook.*\">", RegexOptions.IgnoreCase));
        }

        [Test, AUT(AUT.Uk), JIRA("UKFB-6"), Pending("Waiting on Facebook Environment Setup - AUT.UkFb")]
        public void VerifyFacebookSdkIsIncluded()
        {
            var homePage = Client.Home();
            Assert.IsTrue(Regex.IsMatch(homePage.Source, "connect.facebook.net/en_US/all.js", RegexOptions.IgnoreCase));
        }

    }
}