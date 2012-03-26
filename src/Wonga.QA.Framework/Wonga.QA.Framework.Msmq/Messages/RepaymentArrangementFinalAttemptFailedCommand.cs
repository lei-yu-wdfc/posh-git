using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.RepaymentArrangementFinalAttemptFailedMessage </summary>
    [XmlRoot("RepaymentArrangementFinalAttemptFailedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class RepaymentArrangementFinalAttemptFailedCommand : MsmqMessage<RepaymentArrangementFinalAttemptFailedCommand>
    {
        public Int32 ApplicationId { get; set; }
        public Guid ApplicationGuid { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
        public Guid RepaymentArrangementDetailId { get; set; }
    }
}
