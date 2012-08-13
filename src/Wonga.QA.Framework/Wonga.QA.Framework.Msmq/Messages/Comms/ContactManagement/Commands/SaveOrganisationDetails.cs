using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.Commands
{
    /// <summary> Wonga.Comms.ContactManagement.Commands.SaveOrganisationDetailsMessage </summary>
    [XmlRoot("SaveOrganisationDetailsMessage", Namespace = "Wonga.Comms.ContactManagement.Commands", DataType = "" )
    , SourceAssembly("Wonga.Comms.ContactManagement.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveOrganisationDetails : MsmqMessage<SaveOrganisationDetails>
    {
        public Guid OrganisationId { get; set; }
        public String OrganisationName { get; set; }
        public String RegisteredNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
