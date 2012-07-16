using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreatePreTopUpDocumentResponse </summary>
    [XmlRoot("CreatePreTopUpDocumentResponse", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class CreatePreTopUpDocumentResponse : MsmqMessage<CreatePreTopUpDocumentResponse>
    {
        public String Content { get; set; }
        public Guid SagaId { get; set; }
    }
}
