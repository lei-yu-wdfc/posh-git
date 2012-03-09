using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.Uk.GetAccountOptions </summary>
    [XmlRoot("GetAccountOptions")]
    public partial class GetAccountOptionsUkQuery : ApiRequest<GetAccountOptionsUkQuery>
    {
        public Object AccountId { get; set; }
        public Object TrustRating { get; set; }
    }
}
