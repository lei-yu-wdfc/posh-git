using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.PayLater
{
    public class RiskPayLaterApiCommandsTests
    {
        private readonly dynamic _riskSavePayLaterCustomerDetailsSagaEntity = Drive.Data.OpsSagas.Db.RiskSavePayLaterCustomerDetailsSagaEntity;
        private readonly dynamic _riskSavePayLaterCustomerAddressSagaEntity = Drive.Data.OpsSagas.Db.RiskSavePayLaterCustomerAddressSagaEntity;
        private readonly dynamic _employmentDetails = Drive.Data.Risk.Db.EmploymentDetails;

        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void RiskSavePayLaterCustomerAddress()
        {
            var accountId = Guid.NewGuid();

            Drive.Api.Commands.Post(new RiskSavePayLaterCustomerAddressPayLaterUkCommand
                                        {
                                            AccountId = accountId,
                                            AddressId = Guid.NewGuid(),
                                            Flat = Get.RandomString(5),
                                            HouseNumber = Get.RandomInt(1, 1000),
                                            Postcode = "SW6 6PN",
                                            Street = Get.RandomString(15),
                                            Town = Get.RandomString(10)
                                        });

            Do.Until(() => _riskSavePayLaterCustomerAddressSagaEntity.FindByAccountId(accountId));
        }

        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void RiskSavePayLaterEmploymentDetails()
        {
            var accountId = Guid.NewGuid();

            Drive.Api.Commands.Post(new RiskSavePayLaterEmploymentDetailsPayLaterUkCommand
                                        {
                                            AccountId = accountId,
                                            IncomeFrequency = "LastFridayOfMonth",
                                            NetIncome = 20000.0m,
                                            NextPayDate = Date.GetOrdinalDate(DateTime.Now.AddDays(10), "yyyy-MM-dd"),
                                            EmploymentStatus = "EmployedFullTime"
                                        });

            Do.Until(() => _employmentDetails.FindByAccountId(accountId));
        }

        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void RiskSavePayLaterCustomerDetails()
        {
            var accountId = Guid.NewGuid();

            Drive.Api.Commands.Post(new RiskSavePayLaterCustomerDetailsPayLaterUkCommand
                                        {
                                            AccountId = accountId,
                                            Forename = "Bart",
                                            Surname = "Simpson",
                                            Email = Get.RandomEmail(),
                                            DateOfBirth = Get.GetDoB(),
                                            MobilePhone = Get.GetMobilePhone()
                                        });

            Do.Until(() => _riskSavePayLaterCustomerDetailsSagaEntity.FindByAccountId(accountId));
        }

        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void VerifyApplicationPayLater()
        {
            Drive.Api.Commands.Post(new VerifyPayLaterApplicationUkCommand
                                        {
                                            AccountId = Guid.NewGuid(),
                                            ApplicationId = Guid.NewGuid()
                                        });
        }
    }
}
