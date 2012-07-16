using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.DocumentGeneration.PublicMessages.Wb.Uk
{
    /// <summary> Wonga.Comms.DocumentGeneration.PublicMessages.Wb.Uk.IGuarantorDocumentsInitialEmailCompiled </summary>
    [XmlRoot("IGuarantorDocumentsInitialEmailCompiled", Namespace = "Wonga.Comms.DocumentGeneration.PublicMessages.Wb.Uk", DataType = "")]
    public partial class IGuarantorDocumentsInitialEmailCompiled : MsmqMessage<IGuarantorDocumentsInitialEmailCompiled>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid AccountId { get; set; }
    }
}
