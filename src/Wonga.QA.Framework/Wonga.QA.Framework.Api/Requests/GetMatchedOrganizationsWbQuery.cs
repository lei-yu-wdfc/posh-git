using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetMatchedOrganizations")]
    public partial class GetMatchedOrganizationsWbQuery : ApiRequest<GetMatchedOrganizationsWbQuery>
    {
        public Object Keyword { get; set; }
        public Object City { get; set; }
        public Object PostCode { get; set; }
    }
}
