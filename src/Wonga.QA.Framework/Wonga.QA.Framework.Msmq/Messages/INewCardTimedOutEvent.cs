using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.IEventNewCardCommandTimedOut </summary>
    [XmlRoot("IEventNewCardCommandTimedOut", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "Wonga.PublicMessages.PrepaidCard.IEventNewCardCommand")]
    public partial class INewCardTimedOutEvent : MsmqMessage<INewCardTimedOutEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
