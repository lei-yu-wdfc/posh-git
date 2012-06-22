using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Prepaid.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToCreateSuccessfulPremiumCardCreationEmail </summary>
    [XmlRoot("IWantToCreateSuccessfulPremiumCardCreationEmail", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToCreateSuccessfulPremiumCardCreationEmailUkEvent : MsmqMessage<IWantToCreateSuccessfulPremiumCardCreationEmailUkEvent>
    {
        public Guid AccountId { get; set; }
    }
}
