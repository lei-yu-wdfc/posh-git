using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("StartRiskBusinessVerificationMessage", Namespace = "Wonga.Risk.Workflow.Messages", DataType = "")]
    public class StartRiskBusinessVerificationCommand : MsmqMessage<StartRiskBusinessVerificationCommand>
    {
        public Int32 RiskAccountId { get; set; }
        public Int32 RiskApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public RiskProductEnum RiskProduct { get; set; }
        public Object Context { get; set; }
    }
}
