using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreTopUpAgreementEmail </summary>
    [XmlRoot("IWantToCreateAndStoreTopUpAgreementEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreTopUpAgreementEmail : MsmqMessage<IWantToCreateAndStoreTopUpAgreementEmail>
    {
        public Guid SagaId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TopUpId { get; set; }
    }
}
