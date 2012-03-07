using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.Za.GetLoanAgreementData </summary>
    [XmlRoot("GetLoanAgreementData")]
    public partial class GetLoanAgreementDataZaQuery : ApiRequest<GetLoanAgreementDataZaQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
