using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetNextRepaymentDate </summary>
    [XmlRoot("GetNextRepaymentDate")]
    public partial class GetNextRepaymentDateUkQuery : ApiRequest<GetNextRepaymentDateUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
