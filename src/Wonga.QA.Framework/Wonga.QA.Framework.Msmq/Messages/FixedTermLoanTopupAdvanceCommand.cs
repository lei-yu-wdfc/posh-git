using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.FixedTermLoanTopupAdvanceMessage </summary>
    [XmlRoot("FixedTermLoanTopupAdvanceMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class FixedTermLoanTopupAdvanceCommand : MsmqMessage<FixedTermLoanTopupAdvanceCommand>
    {
        public Int32 TopupId { get; set; }
    }
}
