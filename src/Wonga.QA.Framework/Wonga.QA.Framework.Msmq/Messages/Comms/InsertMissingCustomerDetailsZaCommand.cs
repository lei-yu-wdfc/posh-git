using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("InsertMissingCustomerDetailsMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "")]
    public class InsertMissingCustomerDetailsZaCommand : MsmqMessage<InsertMissingCustomerDetailsZaCommand>
    {
        public Guid AccountId { get; set; }
        public GenderEnum Gender { get; set; }
        public String HomePhone { get; set; }
        public String WorkPhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
