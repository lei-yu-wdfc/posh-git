using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries.Wb.Uk
{
    /// <summary> Wonga.FileStorage.Queries.Wb.Uk.GetFixedInstallmentAgreement </summary>
    [XmlRoot("GetFixedInstallmentAgreement")]
    public partial class GetFixedInstallmentAgreementWbUkQuery : ApiRequest<GetFixedInstallmentAgreementWbUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
