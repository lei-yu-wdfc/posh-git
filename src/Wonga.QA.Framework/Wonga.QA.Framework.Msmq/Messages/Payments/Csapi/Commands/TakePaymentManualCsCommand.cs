using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.Ca;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.TakePaymentManual </summary>
    [XmlRoot("TakePaymentManual", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class TakePaymentManualCsCommand : MsmqMessage<TakePaymentManualCsCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public String SalesforceUser { get; set; }
        public Guid PaymentId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
