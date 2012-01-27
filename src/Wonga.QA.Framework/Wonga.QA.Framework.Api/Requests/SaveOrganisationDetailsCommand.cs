using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SaveOrganisationDetails")]
    public class SaveOrganisationDetailsCommand : ApiRequest<SaveOrganisationDetailsCommand>
    {
        public Object OrganisationId { get; set; }
        public Object OrganisationName { get; set; }
        public Object RegisteredNumber { get; set; }
    }
}
