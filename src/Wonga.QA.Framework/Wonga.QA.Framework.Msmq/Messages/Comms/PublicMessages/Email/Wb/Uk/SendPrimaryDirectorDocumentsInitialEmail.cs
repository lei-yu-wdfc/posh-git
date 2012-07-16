using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Email.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.Email.Wb.Uk.SendPrimaryDirectorDocumentsInitialEmail </summary>
    [XmlRoot("SendPrimaryDirectorDocumentsInitialEmail", Namespace = "Wonga.Comms.PublicMessages.Email.Wb.Uk", DataType = "")]
    public partial class SendPrimaryDirectorDocumentsInitialEmail : MsmqMessage<SendPrimaryDirectorDocumentsInitialEmail>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid AccountId { get; set; }
    }
}
