using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetGuarantorAgreement")]
    public partial class GetGuarantorAgreementWbUkQuery : ApiRequest<GetGuarantorAgreementWbUkQuery>
    {
        public Object ApplicationId { get; set; }
        public Object GuarantorAccountId { get; set; }
    }
}
