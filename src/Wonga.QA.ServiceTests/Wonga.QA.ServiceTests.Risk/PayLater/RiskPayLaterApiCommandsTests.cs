using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.PayLater
{
    [Parallelizable(TestScope.All)]
    public class RiskPayLaterApiCommandsTests
    {
        private Guid _accountId;

        [Test, AUT(AUT.Uk)]
        public void RiskSavePayLaterCustomerDetails()
        {
            _accountId = Guid.NewGuid();

            Drive.Api.Commands.Post(new RiskSavePayLaterCustomerDetailsPayLaterUkCommand
            {
                AccountId = _accountId,
                Forename = "Bart",
                Surname = "Simpson",
                Email = Get.RandomEmail(),
                DateOfBirth = Get.GetDoB(),
                MobilePhone = Get.GetMobilePhone()
            });

            Drive.Msmq.Risk.Send(new IPersonalDetailsAdded {AccountId = _accountId, CreatedOn = DateTime.UtcNow});

            Do.Until(() => Drive.Db.Risk.RiskAccounts.Single(a=> a.AccountId == _accountId));
        }


        [Test, AUT(AUT.Uk), DependsOn("RiskSavePayLaterCustomerDetails")]
        public void RiskSavePayLaterCustomerAddress()
        {
            const string postCode = "SW6 6PN";
            
            Drive.Api.Commands.Post(new RiskSavePayLaterCustomerAddressPayLaterUkCommand
                                        {
                                            AccountId = _accountId,
                                            AddressId = Guid.NewGuid(),
                                            Flat = Get.RandomString(5),
                                            HouseNumber = Get.RandomInt(1, 1000),
                                            Postcode = postCode,
                                            Street = Get.RandomString(15),
                                            Town = Get.RandomString(10)
                                        });

            CreateICurrentAddressAdded();

            RiskAccountEntity riskAccount = null;

            Do.Until(() => riskAccount = Drive.Db.Risk.RiskAccounts.Single(a => a.AccountId == _accountId));

            Assert.IsNotNull(riskAccount.AddressUniqueHash);
        }

        private void CreateICurrentAddressAdded()
        {
            Drive.Msmq.Risk.Send(new ICurrentAddressAdded
            {
                AccountId = _accountId,
            });
        }

        [Test, AUT(AUT.Uk)]
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

            Do.Until(() => Drive.Db.Risk.EmploymentDetails.Single(ed => ed.AccountId == accountId));
        }
    }
}
