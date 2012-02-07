using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateDirectDebitMandateDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate", DataType = "")]
    public partial class CreateDirectDebitMandateDocumentWbUkCommand : MsmqMessage<CreateDirectDebitMandateDocumentWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
