using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("ResendMobilePhonePin")]
    public class ResendMobilePhonePinCommand : ApiRequest<ResendMobilePhonePinCommand>
    {
        public Object VerificationId { get; set; }
        public Object Forename { get; set; }
    }
}
