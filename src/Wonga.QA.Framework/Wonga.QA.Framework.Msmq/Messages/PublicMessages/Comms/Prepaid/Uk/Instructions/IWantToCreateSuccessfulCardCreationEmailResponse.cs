using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.PrepaidCard.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Prepaid.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToCreateSuccessfulCardCreationEmailResponse </summary>
    [XmlRoot("IWantToCreateSuccessfulCardCreationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToCreateSuccessfulCardCreationEmailResponse : MsmqMessage<IWantToCreateSuccessfulCardCreationEmailResponse>
    {
        public Guid DocumentId { get; set; }
        public Guid AccountId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
