using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.RepaymentCollectedMessage </summary>
    [XmlRoot("RepaymentCollectedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class RepaymentCollectedCommand : MsmqMessage<RepaymentCollectedCommand>
    {
        public Guid RepaymentDetailId { get; set; }
        public Boolean PaidInFull { get; set; }
    }
}