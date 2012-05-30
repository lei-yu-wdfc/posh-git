using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToCreateSuccessfulCardCreationEmail </summary>
    [XmlRoot("IWantToCreateSuccessfulCardCreationEmail", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToCreateSuccessfulCardCreationEmailUkEvent : MsmqMessage<IWantToCreateSuccessfulCardCreationEmailUkEvent>
    {
        public Guid AccountId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
