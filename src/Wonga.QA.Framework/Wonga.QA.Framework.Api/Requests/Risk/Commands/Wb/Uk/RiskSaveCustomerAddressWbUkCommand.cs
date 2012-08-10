using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Wb.Uk
{
    /// <summary> Wonga.Risk.Commands.Wb.Uk.RiskSaveCustomerAddress </summary>
    [XmlRoot("RiskSaveCustomerAddress")]
    public partial class RiskSaveCustomerAddressWbUkCommand : ApiRequest<RiskSaveCustomerAddressWbUkCommand>
    {
        public Object AddressId { get; set; }
        public Object AccountId { get; set; }
        public Object Flat { get; set; }
        public Object HouseNumber { get; set; }
        public Object HouseName { get; set; }
        public Object Street { get; set; }
        public Object District { get; set; }
        public Object Town { get; set; }
        public Object County { get; set; }
        public Object Postcode { get; set; }
        public Object SubRegion { get; set; }
    }
}
