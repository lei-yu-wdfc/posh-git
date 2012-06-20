using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.SendPasswordResetEmailMessage </summary>
    [XmlRoot("SendPasswordResetEmailMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class SendPasswordResetEmailCommand : MsmqMessage<SendPasswordResetEmailCommand>
    {
        public Guid NotificationId { get; set; }
        public String UriMask { get; set; }
        public String Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
