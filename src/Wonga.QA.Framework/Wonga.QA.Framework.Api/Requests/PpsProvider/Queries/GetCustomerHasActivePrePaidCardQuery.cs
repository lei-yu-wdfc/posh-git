using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
    /// <summary> Wonga.PpsProvider.Queries.GetCustomerHasActivePrePaidCard </summary>
    [XmlRoot("GetCustomerHasActivePrePaidCard")]
    public partial class GetCustomerHasActivePrePaidCardQuery : ApiRequest<GetCustomerHasActivePrePaidCardQuery>
    {
        public Object AccountId { get; set; }
    }
}
