using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.CreateRepaymentArrangement </summary>
    [XmlRoot("CreateRepaymentArrangement", Namespace = "Wonga.Payments", DataType = "")]
    public partial class CreateRepaymentArrangementCommand : MsmqMessage<CreateRepaymentArrangementCommand>
    {
        public Guid ApplicationId { get; set; }
        public PaymentFrequencyEnum Frequency { get; set; }
        public DateTime[] RepaymentDates { get; set; }
        public Int32 NumberOfMonths { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
