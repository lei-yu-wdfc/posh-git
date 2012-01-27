using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetAccount")]
    public class GetAccountQuery : ApiRequest<GetAccountQuery>
    {
        public Object Login { get; set; }
        public Object Password { get; set; }
    }
}
