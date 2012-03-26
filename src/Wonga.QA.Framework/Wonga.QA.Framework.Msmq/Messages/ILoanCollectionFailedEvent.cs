using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.ILoanCollectionFailed </summary>
    [XmlRoot("ILoanCollectionFailed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IScheduledPaymentFailed,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class ILoanCollectionFailedEvent : MsmqMessage<ILoanCollectionFailedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
