using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendPaymentConfirmationEmail </summary>
    [XmlRoot("IWantToSendPaymentConfirmationEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendPaymentConfirmationEmailEvent : MsmqMessage<IWantToSendPaymentConfirmationEmailEvent>
    {
        public Guid ApplicationId { get; set; }
    }
}
