using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IDecisionDetails", Namespace = "Wonga.Risk.InternalMessages.Events", DataType = "")]
    public partial class IDecisionDetailsEvent : MsmqMessage<IDecisionDetailsEvent>
    {
        public Guid ApplicationId { get; set; }
        public String FailedCheckpoint { get; set; }
        public Int32? RepaymentPredictionScore { get; set; }
    }
}
