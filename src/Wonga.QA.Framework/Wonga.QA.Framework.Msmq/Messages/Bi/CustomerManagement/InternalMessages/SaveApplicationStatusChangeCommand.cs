using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Bi.CustomerManagement.InternalMessages
{
    /// <summary> Wonga.Bi.CustomerManagement.InternalMessages.SaveApplicationStatusChange </summary>
    [XmlRoot("SaveApplicationStatusChange", Namespace = "Wonga.Bi.CustomerManagement.InternalMessages", DataType = "")]
    public partial class SaveApplicationStatusChangeCommand : MsmqMessage<SaveApplicationStatusChangeCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String CurrentStatus { get; set; }
        public String PreviousStatus { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
