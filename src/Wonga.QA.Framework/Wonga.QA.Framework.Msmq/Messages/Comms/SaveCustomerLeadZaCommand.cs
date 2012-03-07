using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.Commands.Za.SaveCustomerLeadMessage </summary>
    [XmlRoot("SaveCustomerLeadMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "")]
    public partial class SaveCustomerLeadZaCommand : MsmqMessage<SaveCustomerLeadZaCommand>
    {
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String Email { get; set; }
        public String MobilePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
