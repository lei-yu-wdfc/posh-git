using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
    /// <summary> Wonga.Ops.Commands.ChangePasswordByTokenAndEmail </summary>
    [XmlRoot("ChangePasswordByTokenAndEmail")]
    public partial class ChangePasswordByTokenAndEmailCommand : ApiRequest<ChangePasswordByTokenAndEmailCommand>
    {
        public Object Token { get; set; }
        public Object Email { get; set; }
        public Object NewPassword { get; set; }
    }
}
