using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IFixedTermApplicationAdded </summary>
    [XmlRoot("IFixedTermApplicationAdded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IApplicationAdded,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IFixedTermApplicationAdded : MsmqMessage<IFixedTermApplicationAdded>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
