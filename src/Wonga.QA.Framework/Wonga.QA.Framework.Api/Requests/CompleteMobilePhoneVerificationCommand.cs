using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("CompleteMobilePhoneVerification")]
    public partial class CompleteMobilePhoneVerificationCommand : ApiRequest<CompleteMobilePhoneVerificationCommand>
    {
        public Object VerificationId { get; set; }
        public Object Pin { get; set; }
    }
}
