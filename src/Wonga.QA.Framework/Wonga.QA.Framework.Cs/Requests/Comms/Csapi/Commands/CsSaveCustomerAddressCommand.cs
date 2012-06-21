using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Comms.Csapi.Commands
{
    /// <summary> Wonga.Comms.Csapi.Commands.CsSaveCustomerAddress </summary>
    [XmlRoot("CsSaveCustomerAddress")]
    public partial class CsSaveCustomerAddressCommand : CsRequest<CsSaveCustomerAddressCommand>
    {
        public Object AccountId { get; set; }
        public Object AddressId { get; set; }
        public Object Postcode { get; set; }
        public Object Town { get; set; }
        public Object Street { get; set; }
        public Object HouseName { get; set; }
        public Object HouseNumber { get; set; }
        public Object Flat { get; set; }
    }
}
