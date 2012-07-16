using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.PrepaidCard.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Prepaid.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToCreateSuccessfulCardCreationEmail </summary>
    [XmlRoot("IWantToCreateSuccessfulCardCreationEmail", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToCreateSuccessfulCardCreationEmail : MsmqMessage<IWantToCreateSuccessfulCardCreationEmail>
    {
        public Guid AccountId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
