using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IUnsignedPersonalGuaranteeAddedToLegalRepository", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.Wb.Uk.IPersonalGuaranteeAddedToLegalRepository")]
    public partial class IUnsignedPersonalGuaranteeAddedToLegalRepositoryWbUkEvent : MsmqMessage<IUnsignedPersonalGuaranteeAddedToLegalRepositoryWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExternalId { get; set; }
    }
}
