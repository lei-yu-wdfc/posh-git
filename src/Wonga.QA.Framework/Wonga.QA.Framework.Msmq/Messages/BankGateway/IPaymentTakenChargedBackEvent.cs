using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("IPaymentTakenChargedBack", Namespace = "Wonga.BankGateway.PublicMessages", DataType = "")]
    public partial class IPaymentTakenChargedBackEvent : MsmqMessage<IPaymentTakenChargedBackEvent>
    {
        public DateTime CreatedOn { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SenderReferenceId { get; set; }
    }
}
