﻿using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Admin
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
