using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IWantToCreateAndStorePreAgreementDocResponse </summary>
    [XmlRoot("IWantToCreateAndStorePreAgreementDocResponse", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IWantToCreateAndStorePreAgreementDocResponseEvent : MsmqMessage<IWantToCreateAndStorePreAgreementDocResponseEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid DocumentId { get; set; }
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
