using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.CreateRepaymentArrangement </summary>
    [XmlRoot("CreateRepaymentArrangement", Namespace = "Wonga.Payments", DataType = "")]
    public partial class CreateRepaymentArrangement : MsmqMessage<CreateRepaymentArrangement>
    {
        public Guid ApplicationId { get; set; }
        public PaymentFrequencyEnum Frequency { get; set; }
        public DateTime[] RepaymentDates { get; set; }
        public Int32 NumberOfMonths { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
