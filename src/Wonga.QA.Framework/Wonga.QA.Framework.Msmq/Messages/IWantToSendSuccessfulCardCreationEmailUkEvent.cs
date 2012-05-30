using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToSendSuccessfulCardCreationEmail </summary>
    [XmlRoot("IWantToSendSuccessfulCardCreationEmail", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToSendSuccessfulCardCreationEmailUkEvent : MsmqMessage<IWantToSendSuccessfulCardCreationEmailUkEvent>
    {
        public CardEnum CardType { get; set; }
        public Guid DocumentId { get; set; }
        public Guid AccountId { get; set; }
    }
}
