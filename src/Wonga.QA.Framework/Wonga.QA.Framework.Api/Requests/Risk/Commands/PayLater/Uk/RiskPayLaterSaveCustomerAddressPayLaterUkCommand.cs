using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    /// <summary> Wonga.Risk.Commands.PayLater.Uk.RiskPayLaterSaveCustomerAddress </summary>
    [XmlRoot("RiskPayLaterSaveCustomerAddress")]
    public partial class RiskPayLaterSaveCustomerAddressPayLaterUkCommand : ApiRequest<RiskPayLaterSaveCustomerAddressPayLaterUkCommand>
    {
        public Object AddressId { get; set; }
        public Object AccountId { get; set; }
        public Object Flat { get; set; }
        public Object HouseNumber { get; set; }
        public Object Street { get; set; }
        public Object Town { get; set; }
        public Object Postcode { get; set; }
    }
}
