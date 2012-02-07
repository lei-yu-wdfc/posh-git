using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreatePreTopUpDocumentMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class CreatePreTopUpDocumentCommand : MsmqMessage<CreatePreTopUpDocumentCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TopupId { get; set; }
    }
}
