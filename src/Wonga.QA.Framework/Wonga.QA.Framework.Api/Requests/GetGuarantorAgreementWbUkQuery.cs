using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.Wb.Uk.GetGuarantorAgreement </summary>
    [XmlRoot("GetGuarantorAgreement")]
    public partial class GetGuarantorAgreementWbUkQuery : ApiRequest<GetGuarantorAgreementWbUkQuery>
    {
        public Object ApplicationId { get; set; }
        public Object GuarantorAccountId { get; set; }
    }
}
