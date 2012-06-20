using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk.BlackList
{
    /// <summary> Wonga.Risk.BlackList.ConsumerBlackListResponseMessage </summary>
    [XmlRoot("ConsumerBlackListResponseMessage", Namespace = "Wonga.Risk.BlackList", DataType = "Wonga.Risk.BlackList.BlackListResponseMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class ConsumerBlackListResponseCommand : MsmqMessage<ConsumerBlackListResponseCommand>
    {
        public Boolean PresentInBlackList { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
