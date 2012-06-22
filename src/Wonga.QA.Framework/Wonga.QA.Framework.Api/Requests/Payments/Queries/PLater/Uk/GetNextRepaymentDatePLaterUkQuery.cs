using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.PLater.Uk
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetNextRepaymentDate </summary>
    [XmlRoot("GetNextRepaymentDate")]
    public partial class GetNextRepaymentDatePLaterUkQuery : ApiRequest<GetNextRepaymentDatePLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
