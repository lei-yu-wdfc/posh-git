using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPaymentCardTypes")]
    public partial class GetPaymentCardTypesQuery : ApiRequest<GetPaymentCardTypesQuery>
    {
    }
}
