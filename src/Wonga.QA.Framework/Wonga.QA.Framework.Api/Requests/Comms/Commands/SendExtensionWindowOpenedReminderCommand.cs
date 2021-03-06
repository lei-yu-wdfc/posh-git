using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.SendExtensionWindowOpenedReminderMessage </summary>
    [XmlRoot("SendExtensionWindowOpenedReminderMessage")]
    public partial class SendExtensionWindowOpenedReminderCommand : ApiRequest<SendExtensionWindowOpenedReminderCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object NotificationDate { get; set; }
        public Object CustomerTimeZoneName { get; set; }
        public Object SendEmailNotification { get; set; }
        public Object SendSmsNotification { get; set; }
    }
}
