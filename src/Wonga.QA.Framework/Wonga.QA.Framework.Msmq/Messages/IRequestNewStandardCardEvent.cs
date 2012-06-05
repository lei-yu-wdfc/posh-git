using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.IRequestNewStandardCardCommand </summary>
    [XmlRoot("IRequestNewStandardCardCommand", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "Wonga.PublicMessages.PrepaidCard.IRequestNewCardCommand")]
    public partial class IRequestNewStandardCardEvent : MsmqMessage<IRequestNewStandardCardEvent>
    {
        public Guid CustomerExternalId { get; set; }
    }
}
