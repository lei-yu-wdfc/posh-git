using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.PublicMessages
{
    /// <summary> Wonga.BankGateway.PublicMessages.IPaymentTakenChargedBack </summary>
    [XmlRoot("IPaymentTakenChargedBack", Namespace = "Wonga.BankGateway.PublicMessages", DataType = "")]
    public partial class IPaymentTakenChargedBack : MsmqMessage<IPaymentTakenChargedBack>
    {
        public DateTime CreatedOn { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SenderReferenceId { get; set; }
    }
}
