using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.InternalMessages.Events
{
    /// <summary> Wonga.Comms.ContactManagement.InternalMessages.Events.IOrganisationRegisteredAddressAddedInternal </summary>
    [XmlRoot("IOrganisationRegisteredAddressAddedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationRegisteredAddressAdded,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent" )
    , SourceAssembly("Wonga.Comms.ContactManagement.InternalMessages.Events, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IOrganisationRegisteredAddressAddedInternal : MsmqMessage<IOrganisationRegisteredAddressAddedInternal>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
