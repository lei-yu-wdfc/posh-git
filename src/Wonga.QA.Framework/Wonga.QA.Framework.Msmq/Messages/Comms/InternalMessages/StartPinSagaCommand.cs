using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.StartPinSagaMessage </summary>
    [XmlRoot("StartPinSagaMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class StartPinSagaCommand : MsmqMessage<StartPinSagaCommand>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public String Forename { get; set; }
    }
}
