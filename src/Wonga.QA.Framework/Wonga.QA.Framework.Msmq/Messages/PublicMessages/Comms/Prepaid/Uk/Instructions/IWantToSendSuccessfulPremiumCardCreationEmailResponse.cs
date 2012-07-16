using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Prepaid.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToSendSuccessfulPremiumCardCreationEmailResponse </summary>
    [XmlRoot("IWantToSendSuccessfulPremiumCardCreationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToSendSuccessfulPremiumCardCreationEmailResponse : MsmqMessage<IWantToSendSuccessfulPremiumCardCreationEmailResponse>
    {
        public Guid AccountId { get; set; }
        public Boolean Successful { get; set; }
    }
}
