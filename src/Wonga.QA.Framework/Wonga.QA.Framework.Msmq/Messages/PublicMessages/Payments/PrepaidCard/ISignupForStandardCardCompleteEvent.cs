using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.ISignupForStandardCardComplete </summary>
    [XmlRoot("ISignupForStandardCardComplete", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard", DataType = "")]
    public partial class ISignupForStandardCardCompleteEvent : MsmqMessage<ISignupForStandardCardCompleteEvent>
    {
        public Guid AccountId { get; set; }
    }
}
