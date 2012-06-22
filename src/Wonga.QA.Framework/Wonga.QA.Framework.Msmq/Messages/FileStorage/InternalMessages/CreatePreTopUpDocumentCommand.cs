using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreatePreTopUpDocument </summary>
    [XmlRoot("CreatePreTopUpDocument", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class CreatePreTopUpDocumentCommand : MsmqMessage<CreatePreTopUpDocumentCommand>
    {
        public Guid TopupId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
