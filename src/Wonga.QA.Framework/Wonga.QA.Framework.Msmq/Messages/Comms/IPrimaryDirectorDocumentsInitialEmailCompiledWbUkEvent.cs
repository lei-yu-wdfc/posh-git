using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IPrimaryDirectorDocumentsInitialEmailCompiled", Namespace = "Wonga.Comms.DocumentGeneration.PublicMessages.Wb.Uk", DataType = "")]
    public partial class IPrimaryDirectorDocumentsInitialEmailCompiledWbUkEvent : MsmqMessage<IPrimaryDirectorDocumentsInitialEmailCompiledWbUkEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid AccountId { get; set; }
    }
}
