using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.FileStorage.InternalMessages;
using Wonga.QA.Framework.Msmq.Enums.Payments.Csapi.Commands;
using Wonga.QA.Framework.Msmq.Enums.Payments.Ca;
using Wonga.QA.Framework.Msmq.Enums.Payments.InternalMessages.Messages;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CreateTransaction </summary>
    [XmlRoot("CreateTransaction", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CreateTransactionCommand : MsmqMessage<CreateTransactionCommand>
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
