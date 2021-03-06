using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetLoanAgreements </summary>
    [XmlRoot("GetLoanAgreements")]
    public partial class GetLoanAgreementsQuery : CsRequest<GetLoanAgreementsQuery>
    {
        public Object AccountId { get; set; }
        public Object IsActive { get; set; }
    }
}
