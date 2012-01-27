using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SaveCustomerLead")]
    public class SaveCustomerLeadCaCommand : ApiRequest<SaveCustomerLeadCaCommand>
    {
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object MobilePhone { get; set; }
        public Object Email { get; set; }
        public Object Province { get; set; }
    }
}
