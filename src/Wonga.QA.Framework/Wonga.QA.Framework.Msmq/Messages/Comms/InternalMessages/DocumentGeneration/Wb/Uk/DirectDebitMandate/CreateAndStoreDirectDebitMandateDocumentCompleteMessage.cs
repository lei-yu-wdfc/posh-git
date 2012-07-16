using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate.CreateAndStoreDirectDebitMandateDocumentCompleteMessage </summary>
    [XmlRoot("CreateAndStoreDirectDebitMandateDocumentCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate", DataType = "")]
    public partial class CreateAndStoreDirectDebitMandateDocumentCompleteMessage : MsmqMessage<CreateAndStoreDirectDebitMandateDocumentCompleteMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileId { get; set; }
    }
}
