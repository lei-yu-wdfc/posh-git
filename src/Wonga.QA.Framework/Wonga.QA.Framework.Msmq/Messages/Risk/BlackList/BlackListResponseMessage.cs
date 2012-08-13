using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.BlackList
{
    /// <summary> Wonga.Risk.BlackList.BlackListResponseMessage </summary>
    [XmlRoot("BlackListResponseMessage", Namespace = "Wonga.Risk.BlackList", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BlackListResponseMessage : MsmqMessage<BlackListResponseMessage>
    {
        public Boolean PresentInBlackList { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
