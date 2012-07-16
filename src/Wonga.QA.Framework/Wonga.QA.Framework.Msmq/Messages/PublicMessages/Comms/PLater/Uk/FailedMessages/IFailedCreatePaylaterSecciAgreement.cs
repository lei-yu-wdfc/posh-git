using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.PLater.Uk.FailedMessages
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.FailedMessages.IFailedCreatePaylaterSecciAgreement </summary>
    [XmlRoot("IFailedCreatePaylaterSecciAgreement", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.FailedMessages", DataType = "")]
    public partial class IFailedCreatePaylaterSecciAgreement : MsmqMessage<IFailedCreatePaylaterSecciAgreement>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
