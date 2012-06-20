using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.ISignupForPremiumCardComplete </summary>
    [XmlRoot("ISignupForPremiumCardComplete", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard", DataType = "")]
    public partial class ISignupForPremiumCardCompleteEvent : MsmqMessage<ISignupForPremiumCardCompleteEvent>
    {
        public Guid AccountId { get; set; }
    }
}
