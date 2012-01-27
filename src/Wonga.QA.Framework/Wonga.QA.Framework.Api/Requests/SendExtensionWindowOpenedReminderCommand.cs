using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SendExtensionWindowOpenedReminderMessage")]
    public class SendExtensionWindowOpenedReminderCommand : ApiRequest<SendExtensionWindowOpenedReminderCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object NotificationDate { get; set; }
        public Object CustomerTimeZoneName { get; set; }
        public Object SendEmailNotification { get; set; }
        public Object SendSmsNotification { get; set; }
    }
}
