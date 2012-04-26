using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendArrearsCommunicationEmail </summary>
    [XmlRoot("IWantToSendArrearsCommunicationEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendArrearsCommunicationEmailEvent : MsmqMessage<IWantToSendArrearsCommunicationEmailEvent>
    {
        public Guid ArrearsCommunicationId { get; set; }
        public ArrearsCommunicationEnum Type { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
