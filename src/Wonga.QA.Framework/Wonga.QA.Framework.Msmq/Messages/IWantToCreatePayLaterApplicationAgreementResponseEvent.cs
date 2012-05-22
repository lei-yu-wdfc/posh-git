using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreatePayLaterApplicationAgreementResponse </summary>
    [XmlRoot("IWantToCreatePayLaterApplicationAgreementResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreatePayLaterApplicationAgreementResponseEvent : MsmqMessage<IWantToCreatePayLaterApplicationAgreementResponseEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
