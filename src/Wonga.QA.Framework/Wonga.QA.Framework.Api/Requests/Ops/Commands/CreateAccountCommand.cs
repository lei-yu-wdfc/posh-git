using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
    /// <summary> Wonga.Ops.Commands.CreateAccount </summary>
    [XmlRoot("CreateAccount")]
    public partial class CreateAccountCommand : ApiRequest<CreateAccountCommand>
    {
        public Object AccountId { get; set; }
        public Object Login { get; set; }
        public Object Password { get; set; }
    }
}
