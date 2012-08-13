using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.URU
{
    /// <summary> Wonga.Risk.URU.CheckAgeUruResponseMessage </summary>
    [XmlRoot("CheckAgeUruResponseMessage", Namespace = "Wonga.Risk.URU", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CheckAgeUruResponseMessage : MsmqMessage<CheckAgeUruResponseMessage>
    {
        public Boolean AgeUnder18 { get; set; }
        public Boolean AgeConfirmed { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
