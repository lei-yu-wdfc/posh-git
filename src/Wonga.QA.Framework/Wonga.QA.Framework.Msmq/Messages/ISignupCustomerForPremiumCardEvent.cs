using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.Instructions.ISignupCustomerForPremiumCard </summary>
    [XmlRoot("ISignupCustomerForPremiumCard", Namespace = "Wonga.PublicMessages.PrepaidCard.Instructions", DataType = "")]
    public partial class ISignupCustomerForPremiumCardEvent : MsmqMessage<ISignupCustomerForPremiumCardEvent>
    {
        public Guid CustomerExternalId { get; set; }
    }
}
