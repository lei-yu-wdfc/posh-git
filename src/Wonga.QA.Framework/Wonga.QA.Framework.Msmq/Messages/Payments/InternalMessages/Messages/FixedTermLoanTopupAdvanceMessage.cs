using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.FixedTermLoanTopupAdvanceMessage </summary>
    [XmlRoot("FixedTermLoanTopupAdvanceMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class FixedTermLoanTopupAdvanceMessage : MsmqMessage<FixedTermLoanTopupAdvanceMessage>
    {
        public Int32 TopupId { get; set; }
    }
}
