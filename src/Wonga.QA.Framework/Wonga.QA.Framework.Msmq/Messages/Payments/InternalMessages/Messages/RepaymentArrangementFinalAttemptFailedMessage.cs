using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.RepaymentArrangementFinalAttemptFailedMessage </summary>
    [XmlRoot("RepaymentArrangementFinalAttemptFailedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RepaymentArrangementFinalAttemptFailedMessage : MsmqMessage<RepaymentArrangementFinalAttemptFailedMessage>
    {
        public Int32 ApplicationId { get; set; }
        public Guid ApplicationGuid { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
        public Guid RepaymentArrangementDetailId { get; set; }
    }
}
