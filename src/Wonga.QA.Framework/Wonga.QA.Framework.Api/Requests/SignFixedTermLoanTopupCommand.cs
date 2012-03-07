using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.SignFixedTermLoanTopup </summary>
    [XmlRoot("SignFixedTermLoanTopup")]
    public partial class SignFixedTermLoanTopupCommand : ApiRequest<SignFixedTermLoanTopupCommand>
    {
        public Object AccountId { get; set; }
        public Object FixedTermLoanTopupId { get; set; }
    }
}
