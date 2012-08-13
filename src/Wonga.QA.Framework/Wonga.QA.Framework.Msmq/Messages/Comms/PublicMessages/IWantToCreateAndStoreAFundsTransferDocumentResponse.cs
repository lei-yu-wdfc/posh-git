using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IWantToCreateAndStoreAFundsTransferDocumentResponse </summary>
    [XmlRoot("IWantToCreateAndStoreAFundsTransferDocumentResponse", Namespace = "Wonga.Comms.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateAndStoreAFundsTransferDocumentResponse : MsmqMessage<IWantToCreateAndStoreAFundsTransferDocumentResponse>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
