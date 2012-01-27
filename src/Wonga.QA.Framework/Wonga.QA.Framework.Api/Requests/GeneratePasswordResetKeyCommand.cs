using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GeneratePasswordResetKey")]
    public class GeneratePasswordResetKeyCommand : ApiRequest<GeneratePasswordResetKeyCommand>
    {
        public Object NotificationId { get; set; }
        public Object Complexity { get; set; }
        public Object Login { get; set; }
    }
}
