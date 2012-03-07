using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.Ca.SaveCustomerAddressToSalesforceMessage </summary>
    [XmlRoot("SaveCustomerAddressToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce.Ca", DataType = "")]
    public partial class SaveCustomerAddressToSalesforceCaCommand : MsmqMessage<SaveCustomerAddressToSalesforceCaCommand>
    {
        public String PoBox { get; set; }
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
        public CountryCodeEnum CountryCode { get; set; }
        public DateTime AtAddressFrom { get; set; }
        public DateTime? AtAddressTo { get; set; }
    }
}