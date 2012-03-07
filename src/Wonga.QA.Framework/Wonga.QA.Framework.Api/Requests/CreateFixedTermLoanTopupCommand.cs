using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.CreateFixedTermLoanTopup </summary>
    [XmlRoot("CreateFixedTermLoanTopup")]
    public partial class CreateFixedTermLoanTopupCommand : ApiRequest<CreateFixedTermLoanTopupCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object FixedTermLoanTopupId { get; set; }
        public Object TopupAmount { get; set; }
    }
}
