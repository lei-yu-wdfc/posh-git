using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate.CreateAndStoreDirectDebitMandateDocumentMessage </summary>
    [XmlRoot("CreateAndStoreDirectDebitMandateDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate", DataType = "")]
    public partial class CreateAndStoreDirectDebitMandateDocumentMessage : MsmqMessage<CreateAndStoreDirectDebitMandateDocumentMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
