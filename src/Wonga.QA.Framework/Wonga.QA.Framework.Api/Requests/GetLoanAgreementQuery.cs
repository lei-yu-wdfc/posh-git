using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetLoanAgreement")]
    public partial class GetLoanAgreementQuery : ApiRequest<GetLoanAgreementQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
