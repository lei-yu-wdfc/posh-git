using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    public partial class RiskSavePayLaterCustomerAddressPayLaterUkCommand     
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            AddressId = Get.GetId();
            HouseNumber = Get.RandomInt(1, 1000);
            Postcode = "0300";
            Street = "Street";
            Town = "City";
            Flat = null;
        }
    }
}
