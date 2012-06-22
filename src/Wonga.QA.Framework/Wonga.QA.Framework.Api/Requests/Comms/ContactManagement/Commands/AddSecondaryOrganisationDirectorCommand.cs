using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.ContactManagement.Commands
{
    /// <summary> Wonga.Comms.ContactManagement.Commands.AddSecondaryOrganisationDirector </summary>
    [XmlRoot("AddSecondaryOrganisationDirector")]
    public partial class AddSecondaryOrganisationDirectorCommand : ApiRequest<AddSecondaryOrganisationDirectorCommand>
    {
        public Object OrganisationId { get; set; }
        public Object AccountId { get; set; }
        public Object Title { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object Email { get; set; }
    }
}
