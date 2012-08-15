using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.PayLater.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Comms
{
    public class CommsPayLaterApiCommands
    {
        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void SavePayLaterCustomerAddress()
        {
            Drive.Api.Commands.Post(new SavePayLaterCustomerAddressPayLaterUkCommand
                                        {
                                            AddressId = Guid.NewGuid(),
                                            AccountId = Guid.NewGuid(),
                                            Flat = Get.RandomString(5),
                                            HouseNumber = Get.RandomInt(1, 1000),
                                            Postcode = "SW6 6PN",
                                            Street = Get.RandomString(15),
                                            Town = Get.RandomString(10)
                                        });
        }

        [Test, AUT(AUT.Uk), Ignore("Awaiting bug fixes")]
        public void SavePayLaterCustomerDetails()
        {
            Drive.Api.Commands.Post(new SavePayLaterCustomerDetailsPayLaterUkCommand
                                        {
                                            AccountId = Guid.NewGuid(),
                                            DateOfBirth = Get.GetDoB(),
                                            Title = "Mr",
                                            Forename = "Bart",
                                            Surname = "Simpson",
                                            Email = Get.RandomEmail(),
                                            MobilePhone = Get.GetMobilePhone()
                                        });
        }
    }
}
