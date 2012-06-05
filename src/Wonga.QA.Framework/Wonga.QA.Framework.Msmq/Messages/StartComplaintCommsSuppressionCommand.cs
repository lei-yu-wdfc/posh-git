using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Suppressions.StartComplaintCommsSuppression </summary>
    [XmlRoot("StartComplaintCommsSuppression", Namespace = "Wonga.Comms.InternalMessages.Suppressions", DataType = "")]
    public partial class StartComplaintCommsSuppressionCommand : MsmqMessage<StartComplaintCommsSuppressionCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
