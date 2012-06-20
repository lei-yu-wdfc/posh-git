using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.Commands
{
    /// <summary> Wonga.Payments.PrepaidCard.Commands.SignupCustomerForStandardCardCommand </summary>
    [XmlRoot("SignupCustomerForStandardCardCommand", Namespace = "Wonga.Payments.PrepaidCard.Commands", DataType = "")]
    public partial class SignupCustomerForStandardCardCommand : MsmqMessage<SignupCustomerForStandardCardCommand>
    {
        public Guid CustomerExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
