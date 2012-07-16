using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate.CreateDirectDebitMandateDocumentCompleteMessage </summary>
    [XmlRoot("CreateDirectDebitMandateDocumentCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate", DataType = "")]
    public partial class CreateDirectDebitMandateDocumentCompleteMessage : MsmqMessage<CreateDirectDebitMandateDocumentCompleteMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Byte[] Content { get; set; }
    }
}
