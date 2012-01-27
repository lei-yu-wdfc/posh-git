using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPaymentCardTypes")]
    public class GetPaymentCardTypesQuery : ApiRequest<GetPaymentCardTypesQuery>
    {
    }
}
