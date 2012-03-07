using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.ContactManagement.Commands.SaveOrganisationDetails </summary>
    [XmlRoot("SaveOrganisationDetails")]
    public partial class SaveOrganisationDetailsCommand : ApiRequest<SaveOrganisationDetailsCommand>
    {
        public Object OrganisationId { get; set; }
        public Object OrganisationName { get; set; }
        public Object RegisteredNumber { get; set; }
    }
}
