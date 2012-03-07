using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetPaymentCardTypes </summary>
    [XmlRoot("GetPaymentCardTypes")]
    public partial class GetPaymentCardTypesQuery : ApiRequest<GetPaymentCardTypesQuery>
    {
    }
}
