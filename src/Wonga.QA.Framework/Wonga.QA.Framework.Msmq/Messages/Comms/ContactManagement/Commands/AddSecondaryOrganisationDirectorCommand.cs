using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Comms.Commands.Ca;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.Commands
{
    /// <summary> Wonga.Comms.ContactManagement.Commands.AddSecondaryOrganisationDirectorMessage </summary>
    [XmlRoot("AddSecondaryOrganisationDirectorMessage", Namespace = "Wonga.Comms.ContactManagement.Commands", DataType = "")]
    public partial class AddSecondaryOrganisationDirectorCommand : MsmqMessage<AddSecondaryOrganisationDirectorCommand>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public TitleEnum? Title { get; set; }
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
