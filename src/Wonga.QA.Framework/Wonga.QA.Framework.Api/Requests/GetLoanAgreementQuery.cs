using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetLoanAgreement")]
    public class GetLoanAgreementQuery : ApiRequest<GetLoanAgreementQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
