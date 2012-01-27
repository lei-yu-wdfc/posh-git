using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("RepayLoanViaBank")]
    public class RepayLoanViaBankCommand : ApiRequest<RepayLoanViaBankCommand>
    {
        public Object ApplicationId { get; set; }
        public Object CashEntityId { get; set; }
        public Object Amount { get; set; }
        public Object RepaymentRequestId { get; set; }
    }
}
