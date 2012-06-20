using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.SignFixedTermLoanExtension </summary>
    [XmlRoot("SignFixedTermLoanExtension", Namespace = "Wonga.Payments", DataType = "")]
    public partial class SignFixedTermLoanExtensionCommand : MsmqMessage<SignFixedTermLoanExtensionCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
