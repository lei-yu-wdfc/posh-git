using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.GenerateCcinMessage </summary>
    [XmlRoot("GenerateCcinMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class GenerateCcinMessage : MsmqMessage<GenerateCcinMessage>
    {
        public Guid AccountId { get; set; }
    }
}
