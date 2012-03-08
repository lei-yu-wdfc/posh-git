using NHamcrest.Core;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using MbUnit.Framework;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class DeclinedPage : BasePage,IDecisionPage
    {
        public DeclinedPage(UiClient client) : base(client)
        {
            Assert.That(Headers, Has.Item(Ui.Get.DeclinedPage.HeaderText));
        }
    }
}
