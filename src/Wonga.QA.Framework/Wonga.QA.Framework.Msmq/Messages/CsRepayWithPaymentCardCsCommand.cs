using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsRepayWithPaymentCard </summary>
    [XmlRoot("CsRepayWithPaymentCard", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class CsRepayWithPaymentCardCsCommand : MsmqMessage<CsRepayWithPaymentCardCsCommand>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public String CV2 { get; set; }
        public String SalesforceUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
