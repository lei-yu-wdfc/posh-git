using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SaveCustomerLeadMessage", Namespace = "Wonga.Comms.Commands.Ca", DataType = "")]
    public partial class SaveCustomerLeadCaCommand : MsmqMessage<SaveCustomerLeadCaCommand>
    {
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String Email { get; set; }
        public String MobilePhone { get; set; }
        public ProvinceEnum Province { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
