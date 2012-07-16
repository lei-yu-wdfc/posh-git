using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Suppressions
{
    /// <summary> Wonga.Comms.InternalMessages.Suppressions.StartComplaintCommsSuppression </summary>
    [XmlRoot("StartComplaintCommsSuppression", Namespace = "Wonga.Comms.InternalMessages.Suppressions", DataType = "")]
    public partial class StartComplaintCommsSuppression : MsmqMessage<StartComplaintCommsSuppression>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
