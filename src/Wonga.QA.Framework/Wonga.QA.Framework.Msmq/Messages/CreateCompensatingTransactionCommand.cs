using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CreateCompensatingTransaction </summary>
    [XmlRoot("CreateCompensatingTransaction", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CreateCompensatingTransactionCommand : MsmqMessage<CreateCompensatingTransactionCommand>
    {
        public Guid ApplicationId { get; set; }
        public Decimal LoanCapExcessAmount { get; set; }
        public DateTime PostedOn { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Decimal Mir { get; set; }
    }
}
