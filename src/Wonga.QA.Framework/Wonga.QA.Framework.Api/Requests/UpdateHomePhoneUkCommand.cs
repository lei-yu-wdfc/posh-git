using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("UpdateHomePhone")]
    public class UpdateHomePhoneUkCommand : ApiRequest<UpdateHomePhoneUkCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
    }
}
