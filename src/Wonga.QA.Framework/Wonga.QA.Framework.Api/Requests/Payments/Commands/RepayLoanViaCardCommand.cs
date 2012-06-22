using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
    /// <summary> Wonga.Payments.Commands.RepayLoanViaCard </summary>
    [XmlRoot("RepayLoanViaCard")]
    public partial class RepayLoanViaCardCommand : ApiRequest<RepayLoanViaCardCommand>
    {
        public Object PaymentRequestId { get; set; }
        public Object ApplicationId { get; set; }
        public Object PaymentCardId { get; set; }
        public Object Amount { get; set; }
        public Object PaymentCardCv2 { get; set; }
    }
}
