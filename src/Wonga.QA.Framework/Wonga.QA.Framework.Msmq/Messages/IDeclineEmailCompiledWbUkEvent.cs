using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk.IDeclineEmailCompiled </summary>
    [XmlRoot("IDeclineEmailCompiled", Namespace = "Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk", DataType = "")]
    public partial class IDeclineEmailCompiledWbUkEvent : MsmqMessage<IDeclineEmailCompiledWbUkEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid AccountId { get; set; }
    }
}
