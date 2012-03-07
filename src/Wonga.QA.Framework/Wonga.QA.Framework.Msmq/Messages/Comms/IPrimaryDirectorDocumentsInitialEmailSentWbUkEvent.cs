using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.PublicMessages.Email.Wb.Uk.IPrimaryDirectorDocumentsInitialEmailSent </summary>
    [XmlRoot("IPrimaryDirectorDocumentsInitialEmailSent", Namespace = "Wonga.Comms.PublicMessages.Email.Wb.Uk", DataType = "")]
    public partial class IPrimaryDirectorDocumentsInitialEmailSentWbUkEvent : MsmqMessage<IPrimaryDirectorDocumentsInitialEmailSentWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
