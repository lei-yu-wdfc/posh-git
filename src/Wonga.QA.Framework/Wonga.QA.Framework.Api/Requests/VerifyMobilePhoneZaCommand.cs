using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Commands.Za.VerifyMobilePhoneZa </summary>
    [XmlRoot("VerifyMobilePhoneZa")]
    public partial class VerifyMobilePhoneZaCommand : ApiRequest<VerifyMobilePhoneZaCommand>
    {
        public Object VerificationId { get; set; }
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
        public Object Forename { get; set; }
    }
}
