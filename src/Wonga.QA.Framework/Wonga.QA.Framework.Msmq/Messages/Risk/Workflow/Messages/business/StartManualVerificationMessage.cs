using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Risk.Workflow;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Workflow.Messages.business
{
    /// <summary> Wonga.Risk.Workflow.Messages.business.StartManualVerificationMessage </summary>
    [XmlRoot("StartManualVerificationMessage", Namespace = "Wonga.Risk.Workflow.Messages.business", DataType = "")]
    public partial class StartManualVerificationMessage : MsmqMessage<StartManualVerificationMessage>
    {
        public Int32 RiskAccountId { get; set; }
        public Guid WorkflowId { get; set; }
        public Int32 RiskApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public RiskProductEnum RiskProduct { get; set; }
        public Object Context { get; set; }
    }
}
