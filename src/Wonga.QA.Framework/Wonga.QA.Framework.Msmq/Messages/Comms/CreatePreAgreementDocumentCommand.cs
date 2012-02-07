using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreatePreAgreementDocumentMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class CreatePreAgreementDocumentCommand : MsmqMessage<CreatePreAgreementDocumentCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
