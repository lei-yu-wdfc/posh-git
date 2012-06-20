using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IWantToCreateAndStorePreAgreementDoc </summary>
    [XmlRoot("IWantToCreateAndStorePreAgreementDoc", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IWantToCreateAndStorePreAgreementDocEvent : MsmqMessage<IWantToCreateAndStorePreAgreementDocEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
