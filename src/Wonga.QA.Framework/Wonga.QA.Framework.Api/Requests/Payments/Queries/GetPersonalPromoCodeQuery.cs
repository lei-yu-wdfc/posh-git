using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetPersonalPromoCode </summary>
    [XmlRoot("GetPersonalPromoCode")]
    public partial class GetPersonalPromoCodeQuery : ApiRequest<GetPersonalPromoCodeQuery>
    {
        public Object AccountId { get; set; }
    }
}
