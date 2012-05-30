using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.IEventNewPremiumCardCommandDone </summary>
    [XmlRoot("IEventNewPremiumCardCommandDone", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "Wonga.PublicMessages.PrepaidCard.IEventNewCardCommandDone,Wonga.PublicMessages.PrepaidCard.IEventNewCardCommand")]
    public partial class INewPremiumCardDoneEvent : MsmqMessage<INewPremiumCardDoneEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
