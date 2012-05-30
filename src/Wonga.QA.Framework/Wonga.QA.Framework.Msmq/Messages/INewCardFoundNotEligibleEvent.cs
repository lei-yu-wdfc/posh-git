using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.IEventNewCardCommandFoundNotEligible </summary>
    [XmlRoot("IEventNewCardCommandFoundNotEligible", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "Wonga.PublicMessages.PrepaidCard.IEventNewCardCommand")]
    public partial class INewCardFoundNotEligibleEvent : MsmqMessage<INewCardFoundNotEligibleEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
