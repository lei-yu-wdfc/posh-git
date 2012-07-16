using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateExtensionSecciDocumentResponse </summary>
    [XmlRoot("CreateExtensionSecciDocumentResponse", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class CreateExtensionSecciDocumentResponse : MsmqMessage<CreateExtensionSecciDocumentResponse>
    {
        public String Content { get; set; }
        public Guid SagaId { get; set; }
    }
}
