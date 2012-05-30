using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.FailedMessages.IFailedCreatePaylaterLegalAgreement </summary>
    [XmlRoot("IFailedCreatePaylaterLegalAgreement", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.FailedMessages", DataType = "")]
    public partial class IFailedCreatePaylaterLegalAgreementUkEvent : MsmqMessage<IFailedCreatePaylaterLegalAgreementUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
