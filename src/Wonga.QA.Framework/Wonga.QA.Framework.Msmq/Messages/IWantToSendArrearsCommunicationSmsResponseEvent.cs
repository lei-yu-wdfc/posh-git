using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendArrearsCommunicationSMSResponse </summary>
    [XmlRoot("IWantToSendArrearsCommunicationSMSResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendArrearsCommunicationSmsResponseEvent : MsmqMessage<IWantToSendArrearsCommunicationSmsResponseEvent>
    {
        public Guid ArrearsCommunicationId { get; set; }
        public ArrearsSmsCommunicationEnum Type { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Boolean Successful { get; set; }
    }
}
