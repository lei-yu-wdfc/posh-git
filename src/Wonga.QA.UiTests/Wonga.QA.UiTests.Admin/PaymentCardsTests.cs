using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.UiTests.Admin
{
    class PaymentCardsTest : UiTest 
    {
        [Ignore]
        [Test, AUT(AUT.Uk)]
        public void CustomerOnAboutUsPageShouldBeAbleChooseEveryLink()
        {
            Client.PaymentCards().AddCard();
        }
    }
}
