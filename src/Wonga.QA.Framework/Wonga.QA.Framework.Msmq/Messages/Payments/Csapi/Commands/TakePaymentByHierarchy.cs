using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.TakePaymentByHierarchy </summary>
    [XmlRoot("TakePaymentByHierarchy", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class TakePaymentByHierarchy : MsmqMessage<TakePaymentByHierarchy>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public String SalesforceUser { get; set; }
        public String CV2 { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
