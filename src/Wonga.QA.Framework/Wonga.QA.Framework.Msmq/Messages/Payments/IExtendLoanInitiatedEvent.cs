using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("IExtendLoanInitiated", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IExtendLoanInitiatedEvent : MsmqMessage<IExtendLoanInitiatedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
