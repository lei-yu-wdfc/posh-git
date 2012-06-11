using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.FileStorage.Queries.GetLoanTopUpAgreement </summary>
    [XmlRoot("GetLoanTopUpAgreement")]
    public partial class GetLoanTopUpAgreementQuery : ApiRequest<GetLoanTopUpAgreementQuery>
    {
        public Object AccountId { get; set; }
        public Object FixedTermLoanTopupId { get; set; }
    }
}
