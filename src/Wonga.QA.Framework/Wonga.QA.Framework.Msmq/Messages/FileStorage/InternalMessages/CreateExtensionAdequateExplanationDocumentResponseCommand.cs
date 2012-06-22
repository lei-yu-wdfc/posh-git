using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateExtensionAdequateExplanationDocumentResponse </summary>
    [XmlRoot("CreateExtensionAdequateExplanationDocumentResponse", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class CreateExtensionAdequateExplanationDocumentResponseCommand : MsmqMessage<CreateExtensionAdequateExplanationDocumentResponseCommand>
    {
        public String Content { get; set; }
        public Guid SagaId { get; set; }
    }
}
