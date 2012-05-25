using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.ContactManagement.Queries.GetOrganisationDetails </summary>
    [XmlRoot("GetOrganisationDetails")]
    public partial class GetOrganisationDetailsQuery : ApiRequest<GetOrganisationDetailsQuery>
    {
        public Object OrganisationId { get; set; }
    }
}
