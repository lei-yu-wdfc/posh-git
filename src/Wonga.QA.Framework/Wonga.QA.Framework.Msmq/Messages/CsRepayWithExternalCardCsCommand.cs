using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsRepayWithExternalCard </summary>
    [XmlRoot("CsRepayWithExternalCard", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class CsRepayWithExternalCardCsCommand : MsmqMessage<CsRepayWithExternalCardCsCommand>
    {
        public Guid AccountId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public String CardType { get; set; }
        public String CardNumber { get; set; }
        public String CV2 { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public String IssueNo { get; set; }
        public String HolderName { get; set; }
        public String AddressLine1 { get; set; }
        public String AddressLine2 { get; set; }
        public String Town { get; set; }
        public String County { get; set; }
        public String PostCode { get; set; }
        public String Country { get; set; }
        public String SalesforceUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
