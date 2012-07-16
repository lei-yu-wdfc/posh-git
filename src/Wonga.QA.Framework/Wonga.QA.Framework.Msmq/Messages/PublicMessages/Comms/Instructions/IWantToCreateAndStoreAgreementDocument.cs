using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreAgreementDocument </summary>
    [XmlRoot("IWantToCreateAndStoreAgreementDocument", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreAgreementDocument : MsmqMessage<IWantToCreateAndStoreAgreementDocument>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
