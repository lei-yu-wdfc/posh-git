using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.WorkflowDecisions
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IWorkflowDecision </summary>
    [XmlRoot("IWorkflowDecision", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWorkflowDecision : MsmqMessage<IWorkflowDecision>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
