using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.PrepaidCard.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Prepaid.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToSendSuccessfulCardCreationEmailResponse </summary>
    [XmlRoot("IWantToSendSuccessfulCardCreationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToSendSuccessfulCardCreationEmailResponse : MsmqMessage<IWantToSendSuccessfulCardCreationEmailResponse>
    {
        public CardEnum CardType { get; set; }
        public Guid AccountId { get; set; }
    }
}
