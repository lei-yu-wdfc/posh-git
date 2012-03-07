using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk.ICompiledPrimaryDirectorCollatedSignedDocumentsEmail </summary>
    [XmlRoot("ICompiledPrimaryDirectorCollatedSignedDocumentsEmail", Namespace = "Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk", DataType = "")]
    public partial class ICompiledPrimaryDirectorCollatedSignedDocumentsEmailWbUkEvent : MsmqMessage<ICompiledPrimaryDirectorCollatedSignedDocumentsEmailWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid CollatedDocumentFileId { get; set; }
    }
}
