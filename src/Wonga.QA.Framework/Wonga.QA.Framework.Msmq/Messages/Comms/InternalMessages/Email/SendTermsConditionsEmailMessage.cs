using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendTermsConditionsEmailMessage </summary>
    [XmlRoot("SendTermsConditionsEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public partial class SendTermsConditionsEmailMessage : MsmqMessage<SendTermsConditionsEmailMessage>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
