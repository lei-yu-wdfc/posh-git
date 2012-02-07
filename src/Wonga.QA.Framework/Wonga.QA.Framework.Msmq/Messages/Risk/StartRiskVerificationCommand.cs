using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("StartRiskVerificationMessage", Namespace = "Wonga.Risk.Workflow.Messages", DataType = "")]
    public partial class StartRiskVerificationCommand : MsmqMessage<StartRiskVerificationCommand>
    {
        public Int32 RiskAccountId { get; set; }
        public Int32 RiskApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public RiskProductEnum RiskProduct { get; set; }
        public Object Context { get; set; }
    }
}
