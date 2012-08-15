using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Payments.PayLater.Commands.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Payments.PayLater
{
    public class PaymentsPayLaterApiCommandsTests
    {
        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void CreateApplication()
        {
            /*Drive.Api.Commands.Post(new CreateApplicationPayLaterUkCommand
                                        {
                                            AccountId = Guid.NewGuid(),
                                            ApplicationId = Guid.NewGuid(),
                                            MerchantId = Guid.NewGuid(),
                                            MerchantReference = "merchantRef",
                                            MerchantOrderId = "merchantOrderId",
                                            TotalAmount = 100.00m,
                                            Currency = CurrencyCodeEnum.GBP,
                                            PostCode = "SW6 6PN"
                                        });*/
        }
    }
}
