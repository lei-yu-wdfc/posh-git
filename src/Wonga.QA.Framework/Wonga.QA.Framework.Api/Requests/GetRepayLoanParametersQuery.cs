using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetRepayLoanParameters </summary>
    [XmlRoot("GetRepayLoanParameters")]
    public partial class GetRepayLoanParametersQuery : ApiRequest<GetRepayLoanParametersQuery>
    {
        public Object AccountId { get; set; }
    }
}
