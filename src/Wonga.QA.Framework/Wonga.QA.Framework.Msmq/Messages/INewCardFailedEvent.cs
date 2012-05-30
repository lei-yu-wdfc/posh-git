using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.IEventNewCardCommandFailed </summary>
    [XmlRoot("IEventNewCardCommandFailed", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "Wonga.PublicMessages.PrepaidCard.IEventNewCardCommand")]
    public partial class INewCardFailedEvent : MsmqMessage<INewCardFailedEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
