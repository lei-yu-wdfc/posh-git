using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.ResendMobilePhonePin </summary>
    [XmlRoot("ResendMobilePhonePin")]
    public partial class ResendMobilePhonePinCommand : ApiRequest<ResendMobilePhonePinCommand>
    {
        public Object VerificationId { get; set; }
        public Object Forename { get; set; }
    }
}
