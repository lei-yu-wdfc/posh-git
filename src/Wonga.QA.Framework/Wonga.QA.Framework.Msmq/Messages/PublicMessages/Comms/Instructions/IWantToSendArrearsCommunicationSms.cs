using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendArrearsCommunicationSms </summary>
    [XmlRoot("IWantToSendArrearsCommunicationSms", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendArrearsCommunicationSms : MsmqMessage<IWantToSendArrearsCommunicationSms>
    {
        public Guid ArrearsCommunicationId { get; set; }
        public ArrearsSmsCommunicationEnum Type { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
