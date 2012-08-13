using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.RepayLoanInternalViaCard </summary>
    [XmlRoot("RepayLoanInternalViaCard", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RepayLoanInternalViaCard : MsmqMessage<RepayLoanInternalViaCard>
    {
        public Object PaymentCardCv2 { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid? CashEntityId { get; set; }
        public Decimal Amount { get; set; }
        public Guid RepaymentRequestId { get; set; }
    }
}
