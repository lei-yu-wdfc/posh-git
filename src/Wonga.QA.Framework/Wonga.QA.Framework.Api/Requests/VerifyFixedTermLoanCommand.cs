using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.VerifyFixedTermLoan </summary>
    [XmlRoot("VerifyFixedTermLoan")]
    public partial class VerifyFixedTermLoanCommand : ApiRequest<VerifyFixedTermLoanCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
