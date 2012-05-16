using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.PLater.Uk.Instructions.IWantToSendPayLaterApplicationConfirmationEmail </summary>
    [XmlRoot("IWantToSendPayLaterApplicationConfirmationEmail", Namespace = "Wonga.PublicMessages.Comms.PLater.Uk.Instructions", DataType = "")]
    public partial class IWantToSendPayLaterApplicationConfirmationEmailUkEvent : MsmqMessage<IWantToSendPayLaterApplicationConfirmationEmailUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
    }
}