using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("CreateFixedTermLoanTopup")]
    public class CreateFixedTermLoanTopupCommand : ApiRequest<CreateFixedTermLoanTopupCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object FixedTermLoanTopupId { get; set; }
        public Object TopupAmount { get; set; }
    }
}
