using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("VerifyMobilePhone")]
    public class VerifyMobilePhoneUkCommand : ApiRequest<VerifyMobilePhoneUkCommand>
    {
        public Object VerificationId { get; set; }
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
        public Object Forename { get; set; }
    }
}
