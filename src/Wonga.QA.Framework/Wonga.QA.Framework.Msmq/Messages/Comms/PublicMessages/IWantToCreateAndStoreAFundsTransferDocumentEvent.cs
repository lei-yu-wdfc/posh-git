using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IWantToCreateAndStoreAFundsTransferDocument </summary>
    [XmlRoot("IWantToCreateAndStoreAFundsTransferDocument", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IWantToCreateAndStoreAFundsTransferDocumentEvent : MsmqMessage<IWantToCreateAndStoreAFundsTransferDocumentEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid DocumentId { get; set; }
        public String Forename { get; set; }
        public String Email { get; set; }
        public Guid SagaId { get; set; }
    }
}
