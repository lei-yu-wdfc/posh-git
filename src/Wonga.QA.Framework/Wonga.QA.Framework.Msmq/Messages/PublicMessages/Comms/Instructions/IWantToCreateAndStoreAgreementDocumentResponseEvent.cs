using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreAgreementDocumentResponse </summary>
    [XmlRoot("IWantToCreateAndStoreAgreementDocumentResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreAgreementDocumentResponseEvent : MsmqMessage<IWantToCreateAndStoreAgreementDocumentResponseEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
