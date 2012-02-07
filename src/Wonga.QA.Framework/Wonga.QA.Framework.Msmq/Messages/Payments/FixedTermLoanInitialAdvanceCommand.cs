using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("FixedTermLoanInitialAdvanceMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class FixedTermLoanInitialAdvanceCommand : MsmqMessage<FixedTermLoanInitialAdvanceCommand>
    {
        public Int32 ApplicationId { get; set; }
    }
}
