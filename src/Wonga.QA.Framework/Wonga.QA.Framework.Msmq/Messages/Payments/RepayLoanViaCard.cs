using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.RepayLoanViaCard </summary>
    [XmlRoot("RepayLoanViaCard", Namespace = "Wonga.Payments", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RepayLoanViaCard : MsmqMessage<RepayLoanViaCard>
    {
        public Guid PaymentRequestId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Decimal Amount { get; set; }
        public String PaymentCardCv2 { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
