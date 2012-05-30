using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.FailedMessages.IFailedCreatePaylaterApplicationConfirmationEmail </summary>
    [XmlRoot("IFailedCreatePaylaterApplicationConfirmationEmail", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.FailedMessages", DataType = "")]
    public partial class IFailedCreatePaylaterApplicationConfirmationEmailUkEvent : MsmqMessage<IFailedCreatePaylaterApplicationConfirmationEmailUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
