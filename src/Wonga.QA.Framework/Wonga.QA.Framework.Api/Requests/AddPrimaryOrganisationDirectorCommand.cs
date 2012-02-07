using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("AddPrimaryOrganisationDirector")]
    public partial class AddPrimaryOrganisationDirectorCommand : ApiRequest<AddPrimaryOrganisationDirectorCommand>
    {
        public Object OrganisationId { get; set; }
        public Object AccountId { get; set; }
        public Object Title { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object Email { get; set; }
    }
}
