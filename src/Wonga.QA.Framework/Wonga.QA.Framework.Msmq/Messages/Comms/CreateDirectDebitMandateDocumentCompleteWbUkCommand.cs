using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateDirectDebitMandateDocumentCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate", DataType = "")]
    public partial class CreateDirectDebitMandateDocumentCompleteWbUkCommand : MsmqMessage<CreateDirectDebitMandateDocumentCompleteWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Byte[] Content { get; set; }
    }
}
