using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class SurveyTest : UiTest
    {
        [Test, AUT(AUT.Ca), JIRA("CA-1826"), Category(TestCategories.SmokeTest)]
        public void IsSurveyAvailableOnHomePage()
        {
            var page = Client.Home();
            Assert.IsNotNull(page.Survey);
        }

        [Test, AUT(AUT.Ca),JIRA("CA-1826"), Category(TestCategories.SmokeTest)]
        public void IsSurveyHiddenByDefaultOnHomePage()
        {
            var page = Client.Home();
            Assert.IsFalse(page.Survey.IsVisible);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1826"), Category(TestCategories.SmokeTest)]
        public void IsSurveyVisibleAfter15SecondsOnHomePage()
        {
            var page = Client.Home();
            Thread.Sleep(18000);
            Assert.IsTrue(page.Survey.IsVisible);
        }

    
    }
}
