using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetPaymentCardTypes </summary>
    [XmlRoot("GetPaymentCardTypes")]
    public partial class GetPaymentCardTypesQuery : ApiRequest<GetPaymentCardTypesQuery>
    {
    }
}
