using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.CreateTopUpDocumentMessage </summary>
    [XmlRoot("CreateTopUpDocumentMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class CreateTopUpDocumentCommand : MsmqMessage<CreateTopUpDocumentCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TopupId { get; set; }
    }
}
