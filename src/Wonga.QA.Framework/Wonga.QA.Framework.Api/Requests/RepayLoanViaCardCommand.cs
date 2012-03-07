using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.RepayLoanViaCard </summary>
    [XmlRoot("RepayLoanViaCard")]
    public partial class RepayLoanViaCardCommand : ApiRequest<RepayLoanViaCardCommand>
    {
        public Object ApplicationId { get; set; }
        public Object CashEntityId { get; set; }
        public Object Amount { get; set; }
        public Object PaymentCardCv2 { get; set; }
        public Object RepaymentRequestId { get; set; }
    }
}
