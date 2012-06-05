using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Marketing.Commands.SignupCustomerForPremiumCardCommand </summary>
    [XmlRoot("SignupCustomerForPremiumCardCommand", Namespace = "Wonga.Marketing.Commands", DataType = "")]
    public partial class SignupCustomerForPremiumCardCommand : MsmqMessage<SignupCustomerForPremiumCardCommand>
    {
        public Guid CustomerExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
