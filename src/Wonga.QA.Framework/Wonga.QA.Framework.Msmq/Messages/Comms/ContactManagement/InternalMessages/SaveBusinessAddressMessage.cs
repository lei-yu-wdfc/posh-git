using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;
using Wonga.QA.Framework.Msmq.Enums.Integration.ContactManagement.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.InternalMessages
{
    /// <summary> Wonga.Comms.ContactManagement.InternalMessages.SaveBusinessAddressMessage </summary>
    [XmlRoot("SaveBusinessAddressMessage", Namespace = "Wonga.Comms.ContactManagement.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Comms.ContactManagement.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveBusinessAddressMessage : MsmqMessage<SaveBusinessAddressMessage>
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
