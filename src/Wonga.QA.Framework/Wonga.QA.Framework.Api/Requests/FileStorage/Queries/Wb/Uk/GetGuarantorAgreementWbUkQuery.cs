using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries.Wb.Uk
{
    /// <summary> Wonga.FileStorage.Queries.Wb.Uk.GetGuarantorAgreement </summary>
    [XmlRoot("GetGuarantorAgreement")]
    public partial class GetGuarantorAgreementWbUkQuery : ApiRequest<GetGuarantorAgreementWbUkQuery>
    {
        public Object ApplicationId { get; set; }
        public Object GuarantorAccountId { get; set; }
    }
}
