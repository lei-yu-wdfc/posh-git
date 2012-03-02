using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IPersonalGuaranteeAddedToLegalRepository", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "")]
    public partial class IPersonalGuaranteeAddedToLegalRepositoryWbUkEvent : MsmqMessage<IPersonalGuaranteeAddedToLegalRepositoryWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExternalId { get; set; }
    }
}
