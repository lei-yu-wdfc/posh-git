using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToSendSuccessfulCardCreationEmailResponse </summary>
    [XmlRoot("IWantToSendSuccessfulCardCreationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToSendSuccessfulCardCreationEmailResponseUkEvent : MsmqMessage<IWantToSendSuccessfulCardCreationEmailResponseUkEvent>
    {
        public CardEnum CardType { get; set; }
        public Guid AccountId { get; set; }
    }
}
