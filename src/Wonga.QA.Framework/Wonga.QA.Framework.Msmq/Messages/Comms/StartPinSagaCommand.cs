using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("StartPinSagaMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class StartPinSagaCommand : MsmqMessage<StartPinSagaCommand>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public String Forename { get; set; }
    }
}
