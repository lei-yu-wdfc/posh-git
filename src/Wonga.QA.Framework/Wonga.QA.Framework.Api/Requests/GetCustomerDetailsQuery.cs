using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetCustomerDetails")]
    public partial class GetCustomerDetailsQuery : ApiRequest<GetCustomerDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}
