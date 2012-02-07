using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ContactManagement
{
    [XmlRoot("SaveBusinessAddressMessage", Namespace = "Wonga.Comms.ContactManagement.InternalMessages", DataType = "")]
    public partial class SaveBusinessAddressCommand : MsmqMessage<SaveBusinessAddressCommand>
    {
        public Int32 Id { get; set; }
        public Guid ExternalId { get; set; }
        public Guid OrganisationId { get; set; }
        public String Building { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String County { get; set; }
        public String Postcode { get; set; }
        public CountryCodeEnum CountryCode { get; set; }
        public BusinessAddressEnum AddressType { get; set; }
    }
}
