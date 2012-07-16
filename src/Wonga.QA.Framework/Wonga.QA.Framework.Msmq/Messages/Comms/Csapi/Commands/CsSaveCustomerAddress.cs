using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Csapi.Commands
{
    /// <summary> Wonga.Comms.Csapi.Commands.SaveCustomerAddressCsApiMessage </summary>
    [XmlRoot("SaveCustomerAddressCsApiMessage", Namespace = "Wonga.Comms.Csapi.Commands", DataType = "")]
    public partial class CsSaveCustomerAddress : MsmqMessage<CsSaveCustomerAddress>
    {
        public Guid AccountId { get; set; }
        public Guid AddressId { get; set; }
        public String Postcode { get; set; }
        public String Town { get; set; }
        public String Street { get; set; }
        public String HouseName { get; set; }
        public String HouseNumber { get; set; }
        public String Flat { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
