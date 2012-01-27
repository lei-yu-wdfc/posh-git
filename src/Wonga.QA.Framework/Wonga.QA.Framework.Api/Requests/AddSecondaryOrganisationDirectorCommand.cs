using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("AddSecondaryOrganisationDirector")]
    public class AddSecondaryOrganisationDirectorCommand : ApiRequest<AddSecondaryOrganisationDirectorCommand>
    {
        public Object OrganisationId { get; set; }
        public Object AccountId { get; set; }
        public Object Title { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object Email { get; set; }
    }
}
