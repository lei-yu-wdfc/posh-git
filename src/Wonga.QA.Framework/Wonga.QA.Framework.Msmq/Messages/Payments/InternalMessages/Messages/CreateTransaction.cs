using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CreateTransaction </summary>
    [XmlRoot("CreateTransaction", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateTransaction : MsmqMessage<CreateTransaction>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExternalId { get; set; }
        public Guid? ComponentTransactionId { get; set; }
        public DateTime PostedOn { get; set; }
        public PaymentTransactionScopeEnum Scope { get; set; }
        public PaymentTransactionEnum Type { get; set; }
        public Decimal Amount { get; set; }
        public Decimal Mir { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public String Reference { get; set; }
        public PaymentTransactionSourceEnum Source { get; set; }
        public Int32? UserId { get; set; }
    }
}
