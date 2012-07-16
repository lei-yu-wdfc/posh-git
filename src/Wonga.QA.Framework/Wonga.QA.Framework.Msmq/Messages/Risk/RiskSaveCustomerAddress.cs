using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.RiskSaveCustomerAddressCommand </summary>
    [XmlRoot("RiskSaveCustomerAddressCommand", Namespace = "Wonga.Risk", DataType = "")]
    public partial class RiskSaveCustomerAddress : MsmqMessage<RiskSaveCustomerAddress>
    {
        public Guid AddressId { get; set; }
        public Guid AccountId { get; set; }
        public String Flat { get; set; }
        public String HouseNumber { get; set; }
        public String HouseName { get; set; }
        public String Street { get; set; }
        public String District { get; set; }
        public String Town { get; set; }
        public String County { get; set; }
        public String Postcode { get; set; }
        public String SubRegion { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
