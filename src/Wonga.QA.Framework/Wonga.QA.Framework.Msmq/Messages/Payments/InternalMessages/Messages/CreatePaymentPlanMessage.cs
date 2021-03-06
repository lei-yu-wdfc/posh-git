using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CreatePaymentPlanMessage </summary>
    [XmlRoot("CreatePaymentPlanMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreatePaymentPlanMessage : MsmqMessage<CreatePaymentPlanMessage>
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
