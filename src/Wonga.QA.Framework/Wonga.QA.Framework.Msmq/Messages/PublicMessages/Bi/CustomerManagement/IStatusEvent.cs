using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Bi.CustomerManagement
{
    /// <summary> Wonga.PublicMessages.Bi.CustomerManagement.IStatusEvent </summary>
    [XmlRoot("IStatusEvent", Namespace = "Wonga.PublicMessages.Bi.CustomerManagement", DataType = "")]
    public partial class IStatusEvent : MsmqMessage<IStatusEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
