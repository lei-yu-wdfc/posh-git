using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.Instructions.IWantToCreatePayLaterApplicationAgreement </summary>
    [XmlRoot("IWantToCreatePayLaterApplicationAgreement", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.Instructions", DataType = "")]
    public partial class IWantToCreatePayLaterApplicationAgreementUkEvent : MsmqMessage<IWantToCreatePayLaterApplicationAgreementUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
