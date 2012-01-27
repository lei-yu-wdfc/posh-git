using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetLoanAgreementData")]
    public class GetLoanAgreementDataZaQuery : ApiRequest<GetLoanAgreementDataZaQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
