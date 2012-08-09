using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
    /// <summary> Wonga.Payments.PayLater.Queries.Uk.GetNextRepaymentDate </summary>
    [XmlRoot("GetNextRepaymentDate")]
    public partial class GetNextRepaymentDatePayLaterUkQuery : ApiRequest<GetNextRepaymentDatePayLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
