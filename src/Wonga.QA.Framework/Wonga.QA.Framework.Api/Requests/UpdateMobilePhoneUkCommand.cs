using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("UpdateMobilePhone")]
    public class UpdateMobilePhoneUkCommand : ApiRequest<UpdateMobilePhoneUkCommand>
    {
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
    }
}
