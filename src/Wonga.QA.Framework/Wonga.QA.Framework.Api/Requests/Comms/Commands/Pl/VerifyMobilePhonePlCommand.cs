using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Pl
{
    /// <summary> Wonga.Comms.Commands.Pl.VerifyMobilePhone </summary>
    [XmlRoot("VerifyMobilePhone")]
    public partial class VerifyMobilePhonePlCommand : ApiRequest<VerifyMobilePhonePlCommand>
    {
        public Object VerificationId { get; set; }
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
        public Object Forename { get; set; }
    }
}
