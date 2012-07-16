using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.BlackList
{
    /// <summary> Wonga.Risk.BlackList.ConsumerBlackListRequestMessage </summary>
    [XmlRoot("ConsumerBlackListRequestMessage", Namespace = "Wonga.Risk.BlackList", DataType = "Wonga.Risk.BlackList.BlackListRequestMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class ConsumerBlackListRequestMessage : MsmqMessage<ConsumerBlackListRequestMessage>
    {
        public String EmployerName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
