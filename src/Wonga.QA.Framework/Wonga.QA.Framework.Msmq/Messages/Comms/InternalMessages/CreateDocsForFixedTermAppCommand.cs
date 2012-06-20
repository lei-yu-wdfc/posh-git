using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.CreateDocsForFixedTermAppMessage </summary>
    [XmlRoot("CreateDocsForFixedTermAppMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class CreateDocsForFixedTermAppCommand : MsmqMessage<CreateDocsForFixedTermAppCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
