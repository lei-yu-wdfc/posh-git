using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SendPasswordResetEmail")]
    public class SendPasswordResetEmailCommand : ApiRequest<SendPasswordResetEmailCommand>
    {
        public Object NotificationId { get; set; }
        public Object Email { get; set; }
        public Object UriMask { get; set; }
    }
}
