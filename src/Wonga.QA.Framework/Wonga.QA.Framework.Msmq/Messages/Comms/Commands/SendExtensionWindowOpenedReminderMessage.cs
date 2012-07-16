using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.SendExtensionWindowOpenedReminderMessage </summary>
    [XmlRoot("SendExtensionWindowOpenedReminderMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class SendExtensionWindowOpenedReminderMessage : MsmqMessage<SendExtensionWindowOpenedReminderMessage>
    {
        public Guid AccountId { get; set; }
        public DateTime NotificationDate { get; set; }
        public Guid ApplicationId { get; set; }
        public String CustomerTimeZoneName { get; set; }
        public Boolean SendEmailNotification { get; set; }
        public Boolean SendSmsNotification { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
