using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("CreatePaymentPlanMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CreatePaymentPlanCommand : MsmqMessage<CreatePaymentPlanCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String BankAccount { get; set; }
        public String BankCode { get; set; }
        public String HolderName { get; set; }
        public Decimal TotalAmount { get; set; }
        public Decimal RegularAmount { get; set; }
        public Decimal LastAmount { get; set; }
        public PaymentFrequencyEnum PaymentFrequency { get; set; }
        public DayOfWeek DayOfWeekForPayment { get; set; }
        public Int32 NumberOfPayments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid SenderReference { get; set; }
    }
}
