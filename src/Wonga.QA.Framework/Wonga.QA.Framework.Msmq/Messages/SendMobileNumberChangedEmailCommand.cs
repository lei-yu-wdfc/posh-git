using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendMobileNumberChangedEmailMessage </summary>
    [XmlRoot("SendMobileNumberChangedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public partial class SendMobileNumberChangedEmailCommand : MsmqMessage<SendMobileNumberChangedEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
