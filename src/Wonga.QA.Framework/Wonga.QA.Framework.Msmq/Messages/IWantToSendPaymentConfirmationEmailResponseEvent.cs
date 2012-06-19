using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendPaymentConfirmationEmailResponse </summary>
    [XmlRoot("IWantToSendPaymentConfirmationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendPaymentConfirmationEmailResponseEvent : MsmqMessage<IWantToSendPaymentConfirmationEmailResponseEvent>
    {
        public Guid ApplicationId { get; set; }
        public Boolean Successful { get; set; }
    }
}
