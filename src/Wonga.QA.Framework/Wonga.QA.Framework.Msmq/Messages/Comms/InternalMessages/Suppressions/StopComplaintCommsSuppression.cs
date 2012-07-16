using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Suppressions
{
    /// <summary> Wonga.Comms.InternalMessages.Suppressions.StopComplaintCommsSuppression </summary>
    [XmlRoot("StopComplaintCommsSuppression", Namespace = "Wonga.Comms.InternalMessages.Suppressions", DataType = "")]
    public partial class StopComplaintCommsSuppression : MsmqMessage<StopComplaintCommsSuppression>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
