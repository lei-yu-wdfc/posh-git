using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Comms.Commands.Ca;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Pl
{
    /// <summary> Wonga.Comms.Commands.Pl.SaveCustomerAddressMessage </summary>
    [XmlRoot("SaveCustomerAddressMessage", Namespace = "Wonga.Comms.Commands.Pl", DataType = "")]
    public partial class SaveCustomerAddressPlCommand : MsmqMessage<SaveCustomerAddressPlCommand>
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
        public CountryCodeEnum CountryCode { get; set; }
        public DateTime AtAddressFrom { get; set; }
        public DateTime? AtAddressTo { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
