using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IRepaymentArrangementPaymentMissedSMSResponse </summary>
    [XmlRoot("IRepaymentArrangementPaymentMissedSMSResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IRepaymentArrangementPaymentMissedSmsResponseEvent : MsmqMessage<IRepaymentArrangementPaymentMissedSmsResponseEvent>
    {
        public Guid AccountId { get; set; }
    }
}
