using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.PublicMessages.CorrelationEvents
{
    /// <summary> Wonga.Comms.ContactManagement.PublicMessages.CorrelationEvents.IApplicationOrganisationPrimaryDirectorCorrelated </summary>
    [XmlRoot("IApplicationOrganisationPrimaryDirectorCorrelated", Namespace = "Wonga.Comms.ContactManagement.PublicMessages.CorrelationEvents", DataType = "Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent" )
    , SourceAssembly("Wonga.Comms.ContactManagement.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IApplicationOrganisationPrimaryDirectorCorrelated : MsmqMessage<IApplicationOrganisationPrimaryDirectorCorrelated>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
