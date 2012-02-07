using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("ResetPassword")]
    public partial class ResetPasswordCommand : ApiRequest<ResetPasswordCommand>
    {
        public Object PwdResetKey { get; set; }
        public Object NewPassword { get; set; }
    }
}
