using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("ManualVerificationDecisionMessage", Namespace = "Wonga.Risk.UI", DataType = "")]
    public partial class ManualVerificationDecisionCommand : MsmqMessage<ManualVerificationDecisionCommand>
    {
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
        public Int32 Probability { get; set; }
    }
}
