using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SignFixedTermLoanExtension")]
    public partial class SignFixedTermLoanExtensionCommand : ApiRequest<SignFixedTermLoanExtensionCommand>
    {
        public Object ApplicationId { get; set; }
        public Object ExtensionId { get; set; }
    }
}
