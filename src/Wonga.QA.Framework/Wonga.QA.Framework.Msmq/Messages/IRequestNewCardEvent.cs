using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.IRequestNewCardCommand </summary>
    [XmlRoot("IRequestNewCardCommand", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "")]
    public partial class IRequestNewCardEvent : MsmqMessage<IRequestNewCardEvent>
    {
        public Guid CustomerExternalId { get; set; }
    }
}
