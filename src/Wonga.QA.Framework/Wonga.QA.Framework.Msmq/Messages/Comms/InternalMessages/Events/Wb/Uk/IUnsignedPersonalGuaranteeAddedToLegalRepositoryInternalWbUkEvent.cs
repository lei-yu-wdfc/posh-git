using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Events.Wb.Uk
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Wb.Uk.IUnsignedPersonalGuaranteeAddedToLegalRepositoryInternal </summary>
    [XmlRoot("IUnsignedPersonalGuaranteeAddedToLegalRepositoryInternal", Namespace = "Wonga.Comms.InternalMessages.Events.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.Wb.Uk.IUnsignedPersonalGuaranteeAddedToLegalRepository,Wonga.Comms.PublicMessages.Wb.Uk.IPersonalGuaranteeAddedToLegalRepository")]
    public partial class IUnsignedPersonalGuaranteeAddedToLegalRepositoryInternalWbUkEvent : MsmqMessage<IUnsignedPersonalGuaranteeAddedToLegalRepositoryInternalWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExternalId { get; set; }
    }
}
