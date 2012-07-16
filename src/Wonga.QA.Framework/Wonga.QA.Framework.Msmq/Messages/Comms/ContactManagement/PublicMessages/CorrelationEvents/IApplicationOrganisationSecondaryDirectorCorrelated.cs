using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.PublicMessages.CorrelationEvents
{
    /// <summary> Wonga.Comms.ContactManagement.PublicMessages.CorrelationEvents.IApplicationOrganisationSecondaryDirectorCorrelated </summary>
    [XmlRoot("IApplicationOrganisationSecondaryDirectorCorrelated", Namespace = "Wonga.Comms.ContactManagement.PublicMessages.CorrelationEvents", DataType = "Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IApplicationOrganisationSecondaryDirectorCorrelated : MsmqMessage<IApplicationOrganisationSecondaryDirectorCorrelated>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public String Forename { get; set; }
        public String Surname { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
