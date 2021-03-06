using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk
{
    /// <summary> Wonga.Payments.Queries.Wb.Uk.GetBusinessPaymentCards </summary>
    [XmlRoot("GetBusinessPaymentCards")]
    public partial class GetBusinessPaymentCardsWbUkQuery : ApiRequest<GetBusinessPaymentCardsWbUkQuery>
    {
        public Object OrganisationId { get; set; }
    }
}
