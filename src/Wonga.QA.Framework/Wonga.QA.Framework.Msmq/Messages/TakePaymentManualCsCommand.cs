using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
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
