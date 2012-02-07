using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("VerifyMobilePhoneZa")]
    public partial class VerifyMobilePhoneZaCommand : ApiRequest<VerifyMobilePhoneZaCommand>
    {
        public Object VerificationId { get; set; }
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
        public Object Forename { get; set; }
    }
}
