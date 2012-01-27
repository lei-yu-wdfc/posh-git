using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("ResendSmsPinMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public class ResendSmsPinCommand : MsmqMessage<ResendSmsPinCommand>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
