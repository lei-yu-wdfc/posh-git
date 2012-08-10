using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
    /// <summary> Wonga.Ops.Commands.ResetPasswordByTokenAndEmail </summary>
    [XmlRoot("ResetPasswordByTokenAndEmail")]
    public partial class ResetPasswordByTokenAndEmailCommand : ApiRequest<ResetPasswordByTokenAndEmailCommand>
    {
        public Object Email { get; set; }
    }
}
