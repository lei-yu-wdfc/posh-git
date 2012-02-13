using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("ChangePassword")]
    public partial class ChangePasswordCommand : ApiRequest<ChangePasswordCommand>
    {
        public Object AccountId { get; set; }
        public Object CurrentPassword { get; set; }
        public Object NewPassword { get; set; }
    }
}
