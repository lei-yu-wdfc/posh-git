using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("IDirectDebitCreated", Namespace = "Wonga.BankGateway.PublicMessages", DataType = "")]
    public partial class IDirectDebitCreatedEvent : MsmqMessage<IDirectDebitCreatedEvent>
    {
        public DateTime CreatedOn { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
