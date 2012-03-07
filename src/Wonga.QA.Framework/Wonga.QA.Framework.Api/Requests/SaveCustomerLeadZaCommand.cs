using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Commands.Za.SaveCustomerLead </summary>
    [XmlRoot("SaveCustomerLead")]
    public partial class SaveCustomerLeadZaCommand : ApiRequest<SaveCustomerLeadZaCommand>
    {
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object MobilePhone { get; set; }
        public Object Email { get; set; }
    }
}
