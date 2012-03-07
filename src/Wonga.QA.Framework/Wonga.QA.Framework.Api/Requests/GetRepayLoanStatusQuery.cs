using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetRepayLoanStatus </summary>
    [XmlRoot("GetRepayLoanStatus")]
    public partial class GetRepayLoanStatusQuery : ApiRequest<GetRepayLoanStatusQuery>
    {
        public Object AccountId { get; set; }
    }
}
