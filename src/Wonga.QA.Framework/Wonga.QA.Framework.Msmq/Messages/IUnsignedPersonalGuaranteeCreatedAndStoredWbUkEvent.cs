using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.DocumentGeneration.PublicMessages.Wb.Uk.IUnsignedPersonalGuaranteeCreatedAndStored </summary>
    [XmlRoot("IUnsignedPersonalGuaranteeCreatedAndStored", Namespace = "Wonga.Comms.DocumentGeneration.PublicMessages.Wb.Uk", DataType = "")]
    public partial class IUnsignedPersonalGuaranteeCreatedAndStoredWbUkEvent : MsmqMessage<IUnsignedPersonalGuaranteeCreatedAndStoredWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid FileId { get; set; }
    }
}
