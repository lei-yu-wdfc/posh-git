using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.Commands
{
    /// <summary> Wonga.Payments.PrepaidCard.Commands.SignupCustomerForPremiumCardCommand </summary>
    [XmlRoot("SignupCustomerForPremiumCardCommand", Namespace = "Wonga.Payments.PrepaidCard.Commands", DataType = "")]
    public partial class SignupCustomerForPremiumCardCommand : MsmqMessage<SignupCustomerForPremiumCardCommand>
    {
        public Guid CustomerExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
