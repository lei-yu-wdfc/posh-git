using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Commands.Ca.SaveCustomerLead </summary>
    [XmlRoot("SaveCustomerLead")]
    public partial class SaveCustomerLeadCaCommand : ApiRequest<SaveCustomerLeadCaCommand>
    {
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object MobilePhone { get; set; }
        public Object Email { get; set; }
        public Object Province { get; set; }
    }
}
