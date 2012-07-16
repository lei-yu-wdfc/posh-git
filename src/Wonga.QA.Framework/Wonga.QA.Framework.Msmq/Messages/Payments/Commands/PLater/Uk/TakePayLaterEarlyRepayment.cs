using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Commands.PLater.Uk
{
    /// <summary> Wonga.Payments.Commands.PLater.Uk.TakePayLaterEarlyRepayment </summary>
    [XmlRoot("TakePayLaterEarlyRepayment", Namespace = "Wonga.Payments.Commands.PLater.Uk", DataType = "")]
    public partial class TakePayLaterEarlyRepayment : MsmqMessage<TakePayLaterEarlyRepayment>
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
