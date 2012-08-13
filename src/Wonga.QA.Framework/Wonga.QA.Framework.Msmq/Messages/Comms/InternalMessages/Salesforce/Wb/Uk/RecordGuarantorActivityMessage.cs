using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce.Wb.Uk
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.Wb.Uk.RecordGuarantorActivityMessage </summary>
    [XmlRoot("RecordGuarantorActivityMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce.Wb.Uk", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Salesforce.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RecordGuarantorActivityMessage : MsmqMessage<RecordGuarantorActivityMessage>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public String ActivityType { get; set; }
        public String Subject { get; set; }
    }
}
