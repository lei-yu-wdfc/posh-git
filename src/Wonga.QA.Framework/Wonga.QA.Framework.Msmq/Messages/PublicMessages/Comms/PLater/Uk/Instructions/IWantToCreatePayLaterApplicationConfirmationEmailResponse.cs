using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.PLater.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.Instructions.IWantToCreatePayLaterApplicationConfirmationEmailResponse </summary>
    [XmlRoot("IWantToCreatePayLaterApplicationConfirmationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.Instructions", DataType = "")]
    public partial class IWantToCreatePayLaterApplicationConfirmationEmailResponse : MsmqMessage<IWantToCreatePayLaterApplicationConfirmationEmailResponse>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
