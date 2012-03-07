using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Wb.Uk.IApplicationOrganisationPrimaryDirectorCorrelated </summary>
    [XmlRoot("IApplicationOrganisationPrimaryDirectorCorrelated", Namespace = "Wonga.Comms.InternalMessages.Events.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.ICommsEvent")]
    public partial class IApplicationOrganisationPrimaryDirectorCorrelatedWbUkEvent : MsmqMessage<IApplicationOrganisationPrimaryDirectorCorrelatedWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
