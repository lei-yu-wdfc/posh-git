using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.PayLater.Uk;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.PayLater
{
    public class RiskPayLaterApiCommandsTests
    {
        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void RiskPayLaterAddPaymentCard()
        {
            Drive.Api.Commands.Post(new RiskPayLaterAddPaymentCardPayLaterUkCommand
                                        {
                                            AccountId = Guid.NewGuid(),
                                            PaymentCardId = Guid.NewGuid(),
                                            ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
                                            Number = "4444333322221111",
                                            SecurityCode = Get.RandomInt(100, 999),
                                        });
        }

        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void RiskPayLaterSaveCustomerAddress()
        {
            Drive.Api.Commands.Post(new RiskPayLaterSaveCustomerAddressPayLaterUkCommand
                                        {
                                            AccountId = Guid.NewGuid(),
                                            AddressId = Guid.NewGuid(),
                                            Flat = Get.RandomString(5),
                                            HouseNumber = Get.RandomInt(1, 1000),
                                            Postcode = "SW6 6PN",
                                            Street = Get.RandomString(15),
                                            Town = Get.RandomString(10)
                                        });
        }

        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void RiskSavePayLaterEmploymentDetails()
        {
            Drive.Api.Commands.Post(new RiskPayLaterSaveEmploymentDetailsPayLaterUkCommand
                                        {
                                            AccountId = Guid.NewGuid(),
                                            IncomeFrequency = "LastFridayOfMonth",
                                            NetIncome = 20000.0m,
                                            NextPayDate = Date.GetOrdinalDate(DateTime.Now.AddDays(10), "yyyy-MM-dd"),
                                            EmploymentStatus = "EmployedFullTime"
                                        });
        }

        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void RiskSavePayLaterCustomerDetails()
        {
            Drive.Api.Commands.Post(new RiskSavePayLaterCustomerDetailsPayLaterUkCommand
                                        {
                                            AccountId = Guid.NewGuid(),
                                            Forename = "Bart",
                                            Surname = "Simpson",
                                            Email = Get.RandomEmail(),
                                            DateOfBirth = Get.GetDoB(),
                                            MobilePhone = Get.GetMobilePhone()
                                        });
        }

        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void VerifyApplication()
        {
            Drive.Api.Commands.Post(new VerifyApplicationPayLaterUkCommand
                                        {
                                            AccountId = Guid.NewGuid(),
                                            ApplicationId = Guid.NewGuid()
                                        });
        }
    }
}
