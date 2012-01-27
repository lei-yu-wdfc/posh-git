using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("VerifyFixedTermLoan")]
    public class VerifyFixedTermLoanCommand : ApiRequest<VerifyFixedTermLoanCommand>
    {
        public Object ApplicationId { get; set; }
        public Object AccountId { get; set; }
    }
}
