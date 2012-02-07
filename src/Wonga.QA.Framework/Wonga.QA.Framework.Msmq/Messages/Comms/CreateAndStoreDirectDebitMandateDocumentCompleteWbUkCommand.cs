using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateAndStoreDirectDebitMandateDocumentCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate", DataType = "")]
    public partial class CreateAndStoreDirectDebitMandateDocumentCompleteWbUkCommand : MsmqMessage<CreateAndStoreDirectDebitMandateDocumentCompleteWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileId { get; set; }
    }
}
