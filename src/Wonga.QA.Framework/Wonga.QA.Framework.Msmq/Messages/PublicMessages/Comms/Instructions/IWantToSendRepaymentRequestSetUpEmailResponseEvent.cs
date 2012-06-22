using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendRepaymentRequestSetUpEmailResponse </summary>
    [XmlRoot("IWantToSendRepaymentRequestSetUpEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendRepaymentRequestSetUpEmailResponseEvent : MsmqMessage<IWantToSendRepaymentRequestSetUpEmailResponseEvent>
    {
        public Guid RepaymentRequestId { get; set; }
        public Boolean Successful { get; set; }
    }
}
