using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
    /// <summary> Wonga.Ops.Commands.GeneratePasswordResetKey </summary>
    [XmlRoot("GeneratePasswordResetKey")]
    public partial class GeneratePasswordResetKeyCommand : ApiRequest<GeneratePasswordResetKeyCommand>
    {
        public Object NotificationId { get; set; }
        public Object Complexity { get; set; }
        public Object Login { get; set; }
    }
}
