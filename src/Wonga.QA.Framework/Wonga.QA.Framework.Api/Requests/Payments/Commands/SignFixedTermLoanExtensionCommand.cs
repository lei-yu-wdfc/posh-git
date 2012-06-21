using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
    /// <summary> Wonga.Payments.Commands.SignFixedTermLoanExtension </summary>
    [XmlRoot("SignFixedTermLoanExtension")]
    public partial class SignFixedTermLoanExtensionCommand : ApiRequest<SignFixedTermLoanExtensionCommand>
    {
        public Object ApplicationId { get; set; }
        public Object ExtensionId { get; set; }
    }
}
