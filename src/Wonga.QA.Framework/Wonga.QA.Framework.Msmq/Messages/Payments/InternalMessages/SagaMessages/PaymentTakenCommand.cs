using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.PaymentTakenMessage </summary>
    [XmlRoot("PaymentTakenMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class PaymentTakenCommand : MsmqMessage<PaymentTakenCommand>
    {
        public Int64 PaymentReference { get; set; }
        public Decimal TransactionAmount { get; set; }
        public DateTime ValueDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Int32 BatchNumber { get; set; }
        public String BankAccountNumber { get; set; }
        public String BankCode { get; set; }
        public DateTime BatchSendTime { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid? TransactionId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
