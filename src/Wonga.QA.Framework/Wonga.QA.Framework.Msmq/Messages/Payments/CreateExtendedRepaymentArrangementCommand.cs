using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("CreateExtendedRepaymentArrangement", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CreateExtendedRepaymentArrangementCommand : MsmqMessage<CreateExtendedRepaymentArrangementCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Decimal EffectiveBalance { get; set; }
        public Decimal RepaymentAmount { get; set; }
        public Object RepaymentDetails { get; set; }
    }
}
