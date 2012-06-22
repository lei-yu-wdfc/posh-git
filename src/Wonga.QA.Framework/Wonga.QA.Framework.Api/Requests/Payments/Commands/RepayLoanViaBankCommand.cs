using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
    /// <summary> Wonga.Payments.Commands.RepayLoanViaBank </summary>
    [XmlRoot("RepayLoanViaBank")]
    public partial class RepayLoanViaBankCommand : ApiRequest<RepayLoanViaBankCommand>
    {
        public Object ApplicationId { get; set; }
        public Object CashEntityId { get; set; }
        public Object Amount { get; set; }
        public Object RepaymentRequestId { get; set; }
        public Object ActionDate { get; set; }
    }
}
