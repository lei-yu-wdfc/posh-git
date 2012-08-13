using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.RepayLoan </summary>
    [XmlRoot("RepayLoan", Namespace = "Wonga.Payments", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RepayLoan : MsmqMessage<RepayLoan>
    {
        public Guid ApplicationId { get; set; }
        public Guid? CashEntityId { get; set; }
        public Decimal? Amount { get; set; }
        public DateTime ActionDate { get; set; }
        public Guid RepaymentRequestId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
