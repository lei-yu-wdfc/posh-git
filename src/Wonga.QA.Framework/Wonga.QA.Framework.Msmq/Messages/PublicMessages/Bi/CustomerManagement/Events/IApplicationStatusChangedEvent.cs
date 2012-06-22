using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Bi.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Bi.CustomerManagement.Events
{
    /// <summary> Wonga.PublicMessages.Bi.CustomerManagement.Events.IApplicationStatusChanged </summary>
    [XmlRoot("IApplicationStatusChanged", Namespace = "Wonga.PublicMessages.Bi.CustomerManagement.Events", DataType = "Wonga.PublicMessages.Bi.CustomerManagement.IStatusEvent")]
    public partial class IApplicationStatusChangedEvent : MsmqMessage<IApplicationStatusChangedEvent>
    {
        public ApplicationStatusEnum Status { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
