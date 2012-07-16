using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateExtensionAdequateExplanationDocument </summary>
    [XmlRoot("CreateExtensionAdequateExplanationDocument", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class CreateExtensionAdequateExplanationDocument : MsmqMessage<CreateExtensionAdequateExplanationDocument>
    {
        public Guid ExtensionId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
