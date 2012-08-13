using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.PublicMessages.CorrelationEvents
{
    /// <summary> Wonga.Comms.ContactManagement.PublicMessages.CorrelationEvents.IApplicationOrganisationCorrelated </summary>
    [XmlRoot("IApplicationOrganisationCorrelated", Namespace = "Wonga.Comms.ContactManagement.PublicMessages.CorrelationEvents", DataType = "Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent" )
    , SourceAssembly("Wonga.Comms.ContactManagement.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IApplicationOrganisationCorrelated : MsmqMessage<IApplicationOrganisationCorrelated>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
