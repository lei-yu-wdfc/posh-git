using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.PayLater.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Comms
{
    [Parallelizable(TestScope.All)]
    public class CommsPayLaterApiCommands
    {
        [Test, AUT(AUT.Uk)]
        public void SavePayLaterCustomerAddress()
        {
            Guid addressId = Guid.NewGuid();

            Drive.Api.Commands.Post(new SavePayLaterCustomerAddressPayLaterUkCommand
                                        {
                                            AddressId = addressId,
                                            AccountId = Guid.NewGuid(),
                                            Flat = Get.RandomString(5),
                                            HouseNumber = Get.RandomInt(1, 1000),
                                            Postcode = "SW6 6PN",
                                            Street = Get.RandomString(15),
                                            Town = Get.RandomString(10)
                                        });

            Do.With.Message("No Customer Address was found").Until(
                () => Drive.Data.Comms.Db.Addresses.FindByExternalId(addressId));
        }

        [Test, AUT(AUT.Uk)]
        public void SavePayLaterCustomerDetails()
        {
            Guid accountId = Guid.NewGuid();

            Drive.Api.Commands.Post(new SavePayLaterCustomerDetailsPayLaterUkCommand
                                        {
                                            AccountId = accountId,
                                            DateOfBirth = Get.GetDoB(),
                                            Title = "Mr",
                                            Forename = "Bart",
                                            Surname = "Simpson",
                                            Email = Get.RandomEmail(),
                                            MobilePhone = Get.GetMobilePhone()
                                        });

            Do.With.Message("No Customer Details were found").Until(
                () => Drive.Data.Comms.Db.CustomerDetails.FindByAccountId(accountId));
        }
    }
}
