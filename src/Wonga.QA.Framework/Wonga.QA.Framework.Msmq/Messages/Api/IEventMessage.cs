using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Api
{
    /// <summary> Wonga.Api.IEventMessage </summary>
    [XmlRoot("IEventMessage", Namespace = "Wonga.Api", DataType = "")]
    public partial class IEventMessage : MsmqMessage<IEventMessage>
    {
        public DateTime CreatedOn { get; set; }
    }
}
