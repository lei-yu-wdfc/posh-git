using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries.Wb
{
    /// <summary> Wonga.Risk.Queries.Wb.GetMatchedOrganizations </summary>
    [XmlRoot("GetMatchedOrganizations")]
    public partial class GetMatchedOrganizationsWbQuery : ApiRequest<GetMatchedOrganizationsWbQuery>
    {
        public Object Keyword { get; set; }
        public Object City { get; set; }
        public Object PostCode { get; set; }
    }
}
