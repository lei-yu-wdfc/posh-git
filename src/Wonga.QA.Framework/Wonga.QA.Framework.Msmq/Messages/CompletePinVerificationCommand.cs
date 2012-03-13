using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.CompletePinVerificationMessage </summary>
    [XmlRoot("CompletePinVerificationMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class CompletePinVerificationCommand : MsmqMessage<CompletePinVerificationCommand>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public String Pin { get; set; }
    }
}
