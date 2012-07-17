using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IHardshipActivated </summary>
    [XmlRoot("IHardshipActivated", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IHardshipActivated : MsmqMessage<IHardshipActivated>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}