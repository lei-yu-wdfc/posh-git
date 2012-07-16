using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IExtensionCancelled </summary>
    [XmlRoot("IExtensionCancelled", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IExtensionCancelled : MsmqMessage<IExtensionCancelled>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
