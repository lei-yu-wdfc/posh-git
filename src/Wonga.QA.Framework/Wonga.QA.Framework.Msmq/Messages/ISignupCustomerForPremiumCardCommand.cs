using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Marketing.Instructions.SignupCustomerForPremiumCard </summary>
    [XmlRoot("SignupCustomerForPremiumCard", Namespace = "Wonga.Marketing.Instructions", DataType = "Wonga.PublicMessages.PrepaidCard.Instructions.ISignupCustomerForPremiumCard")]
    public partial class ISignupCustomerForPremiumCardCommand : MsmqMessage<ISignupCustomerForPremiumCardCommand>
    {
        public Guid CustomerExternalId { get; set; }
    }
}
