using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.PayLater.Uk
{
    public partial class SavePayLaterCustomerAddressPayLaterUkCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            AddressId = Get.GetId();
            HouseNumber = Get.RandomInt(1, 1000);
            Postcode = "SW6 6PN";
            Street = "Street";
            Town = "Town";
        }
    }
}
