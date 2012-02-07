using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("VerifyMobilePhone")]
    public partial class VerifyMobilePhoneCaCommand : ApiRequest<VerifyMobilePhoneCaCommand>
    {
        public Object VerificationId { get; set; }
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
        public Object Forename { get; set; }
    }
}
