using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.InternalMessages.Events
{
    /// <summary> Wonga.Comms.ContactManagement.InternalMessages.Events.IOrganisationVatNumberAddedInternal </summary>
    [XmlRoot("IOrganisationVatNumberAddedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationVatNumberAdded,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent" )
    , SourceAssembly("Wonga.Comms.ContactManagement.InternalMessages.Events, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IOrganisationVatNumberAddedInternal : MsmqMessage<IOrganisationVatNumberAddedInternal>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
