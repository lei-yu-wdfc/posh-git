using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IInArrearsAdded </summary>
    [XmlRoot("IInArrearsAdded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IInArrearsAdded : MsmqMessage<IInArrearsAdded>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime DueDateUtc { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
