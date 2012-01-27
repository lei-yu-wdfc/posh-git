using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SendPasswordResetEmailMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public class SendPasswordResetEmailCommand : MsmqMessage<SendPasswordResetEmailCommand>
    {
        public Guid NotificationId { get; set; }
        public String UriMask { get; set; }
        public String Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
