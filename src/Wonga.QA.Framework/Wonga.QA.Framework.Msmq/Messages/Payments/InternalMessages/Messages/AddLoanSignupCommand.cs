using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AddLoanSignupMessage </summary>
    [XmlRoot("AddLoanSignupMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class AddLoanSignupCommand : MsmqMessage<AddLoanSignupCommand>
    {
        public Guid ExternalId { get; set; }
        public DateTime Date { get; set; }
    }
}
