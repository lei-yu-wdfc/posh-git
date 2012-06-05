using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.IEventNewCardCommand </summary>
    [XmlRoot("IEventNewCardCommand", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "")]
    public partial class INewCardEvent : MsmqMessage<INewCardEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
