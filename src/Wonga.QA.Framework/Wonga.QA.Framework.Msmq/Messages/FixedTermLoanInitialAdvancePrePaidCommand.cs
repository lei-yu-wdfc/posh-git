using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.FixedTermLoanInitialAdvancePrePaidMessage </summary>
    [XmlRoot("FixedTermLoanInitialAdvancePrePaidMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class FixedTermLoanInitialAdvancePrePaidCommand : MsmqMessage<FixedTermLoanInitialAdvancePrePaidCommand>
    {
        public Int32 ApplicationId { get; set; }
    }
}
