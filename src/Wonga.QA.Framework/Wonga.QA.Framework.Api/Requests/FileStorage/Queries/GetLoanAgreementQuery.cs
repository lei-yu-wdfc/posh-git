using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries
{
    /// <summary> Wonga.FileStorage.Queries.GetLoanAgreement </summary>
    [XmlRoot("GetLoanAgreement")]
    public partial class GetLoanAgreementQuery : ApiRequest<GetLoanAgreementQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
