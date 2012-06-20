using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Prepaid.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToSendSuccessfulStandartCardCreationEmail </summary>
    [XmlRoot("IWantToSendSuccessfulStandartCardCreationEmail", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToSendSuccessfulStandartCardCreationEmailUkEvent : MsmqMessage<IWantToSendSuccessfulStandartCardCreationEmailUkEvent>
    {
        public Guid DocumentId { get; set; }
        public Guid AccountId { get; set; }
    }
}
