using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToCreateSuccessfulCardCreationEmailResponse </summary>
    [XmlRoot("IWantToCreateSuccessfulCardCreationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToCreateSuccessfulCardCreationEmailResponseUkEvent : MsmqMessage<IWantToCreateSuccessfulCardCreationEmailResponseUkEvent>
    {
        public Guid DocumentId { get; set; }
        public Guid AccountId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
