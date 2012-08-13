using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PayLater.Commands.Uk
{
    /// <summary> Wonga.Payments.PayLater.Commands.Uk.TakeEarlyRepayment </summary>
    [XmlRoot("TakeEarlyRepayment", Namespace = "Wonga.Payments.PayLater.Commands.Uk", DataType = "" )
    , SourceAssembly("Wonga.Payments.PayLater.Commands.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class TakeEarlyRepayment : MsmqMessage<TakeEarlyRepayment>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal Amount { get; set; }
        public Guid PaymentCardId { get; set; }
        public String Cv2 { get; set; }
        public Guid PaymentRequestId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
