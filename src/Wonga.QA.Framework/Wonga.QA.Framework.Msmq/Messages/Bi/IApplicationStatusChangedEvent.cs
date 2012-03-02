using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Bi
{
    [XmlRoot("IApplicationStatusChanged", Namespace = "Wonga.PublicMessages.Bi.CustomerManagement.Events", DataType = "Wonga.PublicMessages.Bi.CustomerManagement.ISatausEvent")]
    public partial class IApplicationStatusChangedEvent : MsmqMessage<IApplicationStatusChangedEvent>
    {
        public ApplicationStatusEnum Status { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
