using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.Commands
{
    /// <summary> Wonga.Comms.ContactManagement.Commands.AddPrimaryOrganisationDirectorMessage </summary>
    [XmlRoot("AddPrimaryOrganisationDirectorMessage", Namespace = "Wonga.Comms.ContactManagement.Commands", DataType = "")]
    public partial class AddPrimaryOrganisationDirectorCommand : MsmqMessage<AddPrimaryOrganisationDirectorCommand>
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
