using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendArrearsBalanceEmail </summary>
    [XmlRoot("IWantToSendArrearsBalanceEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToSendArrearsBalanceEmailEvent : MsmqMessage<IWantToSendArrearsBalanceEmailEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public ArrearsBalanceEmailEnum EmailType { get; set; }
        public Guid SagaId { get; set; }
    }
}
