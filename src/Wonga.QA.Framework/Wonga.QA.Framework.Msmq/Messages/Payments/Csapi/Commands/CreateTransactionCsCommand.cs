using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.CreateTransaction </summary>
    [XmlRoot("CreateTransaction", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class CreateTransactionCsCommand : MsmqMessage<CreateTransactionCsCommand>
    {
        public Guid ApplicationGuid { get; set; }
        public PaymentTransactionScopeEnum Scope { get; set; }
        public PaymentTransactionEnum Type { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public String Reference { get; set; }
        public String SalesForceUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
