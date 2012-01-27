using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("UpdateCustomerGenderMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "")]
    public class UpdateCustomerGenderZaCommand : MsmqMessage<UpdateCustomerGenderZaCommand>
    {
        public Guid AccountId { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
