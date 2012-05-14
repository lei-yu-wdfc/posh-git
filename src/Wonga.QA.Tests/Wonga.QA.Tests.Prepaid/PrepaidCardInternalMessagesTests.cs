using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Prepaid
{
    public class PrepaidCardInternalMessagesTests
    {
        private static readonly String STANDARD_CARD_TEMPLATE_NAME = "34327";
        private static readonly String PREMIUM_CARD_TEMPLATE_NAME = "34328";
        private static readonly String STANDARD_CARD_TYPE = "Standard";
        private static readonly String PREMIUM_CARD_TYPE = "Premium";

        private static readonly dynamic _qaDataDb = Drive.Data.QaData.Db;

        [Test, AUT(AUT.Uk), JIRA("PP-11")]
        public void SendCardsEventForReceivingEmail()
        {
            Customer standardCardCustomer = CustomerBuilder.New().Build();
            Customer premiumCardCustomer = CustomerBuilder.New().Build();

            var requestToStandardCard = new INewStandardCardCommandDoneEvent();
            var requestToPremiumCard = new INewPremiumCardCommandDoneEvent();
            var invalidRequestToStandardCard = new INewStandardCardCommandDoneEvent();
            var invalidRequestToPremiumCard = new INewPremiumCardCommandDoneEvent();

            requestToStandardCard.CustomerExternalId = standardCardCustomer.Id;
            requestToPremiumCard.CardType = STANDARD_CARD_TYPE;
            requestToPremiumCard.CustomerExternalId = premiumCardCustomer.Id;
            requestToPremiumCard.CardType = PREMIUM_CARD_TYPE;

            invalidRequestToPremiumCard.CustomerExternalId = Guid.NewGuid();
            invalidRequestToPremiumCard.CardType = PREMIUM_CARD_TYPE;
            invalidRequestToStandardCard.CustomerExternalId = Guid.NewGuid();
            invalidRequestToStandardCard.CardType = STANDARD_CARD_TYPE;

            Drive.Msmq.Comms.Send(requestToStandardCard);
            Drive.Msmq.Comms.Send(requestToPremiumCard);
            Drive.Msmq.Comms.Send(invalidRequestToStandardCard);
            Drive.Msmq.Comms.Send(invalidRequestToPremiumCard);

            Do.Until(() => _qaDataDb.Email.FindBy(EmailAddress:standardCardCustomer.GetEmail(),TemplateName:STANDARD_CARD_TEMPLATE_NAME));
            Do.Until(() => _qaDataDb.Email.FindBy(EmailAddress:premiumCardCustomer.GetEmail(), TemplateName:PREMIUM_CARD_TEMPLATE_NAME));
        }
    }
}