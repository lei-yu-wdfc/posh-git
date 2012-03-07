using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.PublicMessages.Email.Wb.Uk.SendGuarantorDocumentsInitialEmail </summary>
    [XmlRoot("SendGuarantorDocumentsInitialEmail", Namespace = "Wonga.Comms.PublicMessages.Email.Wb.Uk", DataType = "")]
    public partial class SendGuarantorDocumentsInitialEmailWbUkEvent : MsmqMessage<SendGuarantorDocumentsInitialEmailWbUkEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid AccountId { get; set; }
    }
}
