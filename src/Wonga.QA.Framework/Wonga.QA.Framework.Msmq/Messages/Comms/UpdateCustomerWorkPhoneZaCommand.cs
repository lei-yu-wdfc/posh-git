using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("UpdateCustomerWorkPhoneMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "")]
    public class UpdateCustomerWorkPhoneZaCommand : MsmqMessage<UpdateCustomerWorkPhoneZaCommand>
    {
        public Guid AccountId { get; set; }
        public String WorkPhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
