using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.Instructions.IWantToCreatePayLaterApplicationConfirmationEmail </summary>
    [XmlRoot("IWantToCreatePayLaterApplicationConfirmationEmail", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.Instructions", DataType = "")]
    public partial class IWantToCreatePayLaterApplicationConfirmationEmailUkEvent : MsmqMessage<IWantToCreatePayLaterApplicationConfirmationEmailUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
