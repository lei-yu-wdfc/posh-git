using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("UpdateCustomerWorkPhoneZa")]
    public class UpdateCustomerWorkPhoneZaCommand : ApiRequest<UpdateCustomerWorkPhoneZaCommand>
    {
        public Object AccountId { get; set; }
        public Object WorkPhone { get; set; }
    }
}
