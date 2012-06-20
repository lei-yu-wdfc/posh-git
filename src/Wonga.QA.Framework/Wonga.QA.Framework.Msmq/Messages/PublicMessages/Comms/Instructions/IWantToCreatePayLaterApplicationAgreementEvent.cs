using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreatePayLaterApplicationAgreement </summary>
    [XmlRoot("IWantToCreatePayLaterApplicationAgreement", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreatePayLaterApplicationAgreementEvent : MsmqMessage<IWantToCreatePayLaterApplicationAgreementEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
