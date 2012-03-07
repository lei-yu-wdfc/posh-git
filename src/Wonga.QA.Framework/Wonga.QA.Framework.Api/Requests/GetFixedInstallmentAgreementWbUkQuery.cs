using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.Wb.Uk.GetFixedInstallmentAgreement </summary>
    [XmlRoot("GetFixedInstallmentAgreement")]
    public partial class GetFixedInstallmentAgreementWbUkQuery : ApiRequest<GetFixedInstallmentAgreementWbUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
