using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.IPersonalGuaranteeAddedToLegalRepository </summary>
    [XmlRoot("IPersonalGuaranteeAddedToLegalRepository", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "")]
    public partial class IPersonalGuaranteeAddedToLegalRepository : MsmqMessage<IPersonalGuaranteeAddedToLegalRepository>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExternalId { get; set; }
    }
}
