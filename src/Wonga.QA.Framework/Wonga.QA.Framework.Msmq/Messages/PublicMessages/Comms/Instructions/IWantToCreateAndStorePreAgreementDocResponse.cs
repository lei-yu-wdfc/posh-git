using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStorePreAgreementDocResponse </summary>
    [XmlRoot("IWantToCreateAndStorePreAgreementDocResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStorePreAgreementDocResponse : MsmqMessage<IWantToCreateAndStorePreAgreementDocResponse>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
