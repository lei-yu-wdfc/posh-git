using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Workflow.Messages.business.StartBusinessVerificationMessage </summary>
    [XmlRoot("StartBusinessVerificationMessage", Namespace = "Wonga.Risk.Workflow.Messages.business", DataType = "")]
    public partial class StartBusinessVerificationCommand : MsmqMessage<StartBusinessVerificationCommand>
    {
        public Guid OrganisationId { get; set; }
        public Int32 RiskAccountId { get; set; }
        public Guid WorkflowId { get; set; }
        public Int32 RiskApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public RiskProductEnum RiskProduct { get; set; }
        public Object Context { get; set; }
    }
}
