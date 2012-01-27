using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedInstallmentAgreement")]
    public class GetFixedInstallmentAgreementWbUkQuery : ApiRequest<GetFixedInstallmentAgreementWbUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
