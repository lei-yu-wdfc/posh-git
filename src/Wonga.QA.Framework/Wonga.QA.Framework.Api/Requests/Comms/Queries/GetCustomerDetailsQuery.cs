using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
    /// <summary> Wonga.Comms.Queries.GetCustomerDetails </summary>
    [XmlRoot("GetCustomerDetails")]
    public partial class GetCustomerDetailsQuery : ApiRequest<GetCustomerDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}
