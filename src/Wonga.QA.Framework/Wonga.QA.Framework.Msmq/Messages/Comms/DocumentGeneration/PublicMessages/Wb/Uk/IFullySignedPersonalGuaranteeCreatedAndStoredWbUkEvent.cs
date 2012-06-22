using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.DocumentGeneration.PublicMessages.Wb.Uk
{
    /// <summary> Wonga.Comms.DocumentGeneration.PublicMessages.Wb.Uk.IFullySignedPersonalGuaranteeCreatedAndStored </summary>
    [XmlRoot("IFullySignedPersonalGuaranteeCreatedAndStored", Namespace = "Wonga.Comms.DocumentGeneration.PublicMessages.Wb.Uk", DataType = "")]
    public partial class IFullySignedPersonalGuaranteeCreatedAndStoredWbUkEvent : MsmqMessage<IFullySignedPersonalGuaranteeCreatedAndStoredWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid FileId { get; set; }
    }
}
