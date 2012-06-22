using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CancelLoanApplicationMessage </summary>
    [XmlRoot("CancelLoanApplicationMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CancelLoanApplicationCommand : MsmqMessage<CancelLoanApplicationCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
