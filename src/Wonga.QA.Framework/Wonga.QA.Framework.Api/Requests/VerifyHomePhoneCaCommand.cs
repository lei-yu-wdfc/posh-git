using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("VerifyHomePhone")]
    public class VerifyHomePhoneCaCommand : ApiRequest<VerifyHomePhoneCaCommand>
    {
        public Object VerificationId { get; set; }
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
        public Object Forename { get; set; }
    }
}
