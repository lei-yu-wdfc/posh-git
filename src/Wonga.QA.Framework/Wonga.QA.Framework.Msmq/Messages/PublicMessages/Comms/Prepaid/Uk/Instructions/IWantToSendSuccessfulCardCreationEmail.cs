using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.PrepaidCard.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Prepaid.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToSendSuccessfulCardCreationEmail </summary>
    [XmlRoot("IWantToSendSuccessfulCardCreationEmail", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms.Prepaid.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendSuccessfulCardCreationEmail : MsmqMessage<IWantToSendSuccessfulCardCreationEmail>
    {
        public CardEnum CardType { get; set; }
        public Guid DocumentId { get; set; }
        public Guid AccountId { get; set; }
    }
}
