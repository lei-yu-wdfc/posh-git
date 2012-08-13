using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Workflow.Messages
{
    /// <summary> Wonga.Risk.Workflow.Messages.FinalizeWorkflowProcessingMessage </summary>
    [XmlRoot("FinalizeWorkflowProcessingMessage", Namespace = "Wonga.Risk.Workflow.Messages", DataType = "" )
    , SourceAssembly("Wonga.Risk.Workflow.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class FinalizeWorkflowProcessingMessage : MsmqMessage<FinalizeWorkflowProcessingMessage>
    {
        public Int32 RiskAccountId { get; set; }
        public Int32 RiskApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
