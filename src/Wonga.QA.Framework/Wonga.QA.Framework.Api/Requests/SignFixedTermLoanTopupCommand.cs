using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SignFixedTermLoanTopup")]
    public class SignFixedTermLoanTopupCommand : ApiRequest<SignFixedTermLoanTopupCommand>
    {
        public Object AccountId { get; set; }
        public Object FixedTermLoanTopupId { get; set; }
    }
}
