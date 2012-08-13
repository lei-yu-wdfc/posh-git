using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Msmq.Enums.Payments.PayLater.Data.Enums;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.Enums.Payments.PayLater.Data.Enums.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.Payments.PayLater.Data.Enums.PaymentTransactionScopeEnum;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PayLater.InternalMessages
{
    /// <summary> Wonga.Payments.PayLater.InternalMessages.CreateTransaction </summary>
    [XmlRoot("CreateTransaction", Namespace = "Wonga.Payments.PayLater.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Payments.PayLater.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateTransaction : MsmqMessage<CreateTransaction>
    {
        public Decimal Amount { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid? ComponentTransactionId { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid ExternalId { get; set; }
        public DateTime PostedOn { get; set; }
        public String Reference { get; set; }
        public PaymentTransactionSourceEnum Source { get; set; }
        public PaymentTransactionScopeEnum Scope { get; set; }
        public PaymentTransactionEnum Type { get; set; }
    }
}
