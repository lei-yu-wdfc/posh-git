using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsExtendFixedTermLoan </summary>
    [XmlRoot("CsExtendFixedTermLoan")]
    public partial class CsExtendFixedTermLoanCommand : CsRequest<CsExtendFixedTermLoanCommand>
    {
        public Object ApplicationId { get; set; }
        public Object PaymentCardId { get; set; }
        public Object LoanExtensionId { get; set; }
        public Object CV2 { get; set; }
        public Object PartPaymentAmount { get; set; }
        public Object AgentId { get; set; }
        public Object ExtensionDate { get; set; }
    }
}
