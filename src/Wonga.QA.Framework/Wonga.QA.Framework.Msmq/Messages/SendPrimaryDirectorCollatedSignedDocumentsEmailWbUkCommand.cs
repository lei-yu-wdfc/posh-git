using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Email.InternalMessages.Wb.Uk.SendPrimaryDirectorCollatedSignedDocumentsEmailMessage </summary>
    [XmlRoot("SendPrimaryDirectorCollatedSignedDocumentsEmailMessage", Namespace = "Wonga.Email.InternalMessages.Wb.Uk", DataType = "")]
    public partial class SendPrimaryDirectorCollatedSignedDocumentsEmailWbUkCommand : MsmqMessage<SendPrimaryDirectorCollatedSignedDocumentsEmailWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public String CustomerEmail { get; set; }
        public Guid CollatedDocumentFileId { get; set; }
    }
}
