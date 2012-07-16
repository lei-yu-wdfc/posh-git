using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendArrearsCommunicationEmailResponse </summary>
    [XmlRoot("IWantToSendArrearsCommunicationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendArrearsCommunicationEmailResponse : MsmqMessage<IWantToSendArrearsCommunicationEmailResponse>
    {
        public Guid ArrearsCommunicationId { get; set; }
        public ArrearsCommunicationEnum Type { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Boolean Successful { get; set; }
    }
}
