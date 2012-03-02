using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Bi
{
    [XmlRoot("ISatausEvent", Namespace = "Wonga.PublicMessages.Bi.CustomerManagement", DataType = "")]
    public partial class ISatausEvent : MsmqMessage<ISatausEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
