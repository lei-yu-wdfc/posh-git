using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.IRequestNewPremiumCardCommand </summary>
    [XmlRoot("IRequestNewPremiumCardCommand", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "Wonga.PublicMessages.PrepaidCard.IRequestNewCardCommand")]
    public partial class IRequestNewPremiumCardEvent : MsmqMessage<IRequestNewPremiumCardEvent>
    {
        public Guid CustomerExternalId { get; set; }
    }
}
