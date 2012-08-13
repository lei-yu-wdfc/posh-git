using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IResumeRiskWorkflow </summary>
    [XmlRoot("IResumeRiskWorkflow", Namespace = "Wonga.Risk", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IResumeRiskWorkflow : MsmqMessage<IResumeRiskWorkflow>
    {
        public Guid SagaId { get; set; }
    }
}
