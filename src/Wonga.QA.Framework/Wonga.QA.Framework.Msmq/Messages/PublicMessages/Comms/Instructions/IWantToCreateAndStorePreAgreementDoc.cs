using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStorePreAgreementDoc </summary>
    [XmlRoot("IWantToCreateAndStorePreAgreementDoc", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStorePreAgreementDoc : MsmqMessage<IWantToCreateAndStorePreAgreementDoc>
    {
        public Guid ApplicationId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
