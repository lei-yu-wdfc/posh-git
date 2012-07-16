using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.INotifyA1SmsShouldBeScheduled </summary>
    [XmlRoot("INotifyA1SmsShouldBeScheduled", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class INotifyA1SmsShouldBeScheduled : MsmqMessage<INotifyA1SmsShouldBeScheduled>
    {
        public Guid AccountId { get; set; }
        public DateTime RemindDateUtc { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
