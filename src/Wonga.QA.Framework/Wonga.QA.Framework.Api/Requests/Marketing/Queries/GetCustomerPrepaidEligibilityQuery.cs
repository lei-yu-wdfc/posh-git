using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Queries
{
    /// <summary> Wonga.Marketing.Queries.GetCustomerPrepaidEligibility </summary>
    [XmlRoot("GetCustomerPrepaidEligibility")]
    public partial class GetCustomerPrepaidEligibilityQuery : ApiRequest<GetCustomerPrepaidEligibilityQuery>
    {
        public Object AccountId { get; set; }
    }
}
