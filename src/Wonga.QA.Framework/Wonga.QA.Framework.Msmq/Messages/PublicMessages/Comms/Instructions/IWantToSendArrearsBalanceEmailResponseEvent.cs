using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Comms.Instructions;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendArrearsBalanceEmailResponse </summary>
    [XmlRoot("IWantToSendArrearsBalanceEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToSendArrearsBalanceEmailResponseEvent : MsmqMessage<IWantToSendArrearsBalanceEmailResponseEvent>
    {
        public ArrearsBalanceEmailEnum EmailType { get; set; }
        public Boolean Successful { get; set; }
        public Guid SagaId { get; set; }
    }
}
