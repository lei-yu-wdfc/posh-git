using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("CompleteHomePhoneVerification")]
    public partial class CompleteHomePhoneVerificationCaCommand : ApiRequest<CompleteHomePhoneVerificationCaCommand>
    {
        public Object VerificationId { get; set; }
        public Object Pin { get; set; }
    }
}
