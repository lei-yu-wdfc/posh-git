using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.IEmailSent </summary>
    [XmlRoot("IEmailSent", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "")]
    public partial class IEmailSentWbUkEvent : MsmqMessage<IEmailSentWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
    }
}
