using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.ContactManagement.Queries.GetOrganisationGuarantors </summary>
    [XmlRoot("GetOrganisationGuarantors")]
    public partial class GetOrganisationGuarantorsQuery : ApiRequest<GetOrganisationGuarantorsQuery>
    {
        public Object OrganisationId { get; set; }
    }
}
