using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetLoanExtensionAgreement")]
    public partial class GetLoanExtensionAgreementQuery : ApiRequest<GetLoanExtensionAgreementQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
