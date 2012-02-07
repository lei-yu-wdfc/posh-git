using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateAgreeementDocumentMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class CreateAgreeementDocumentCommand : MsmqMessage<CreateAgreeementDocumentCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
