using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.StartEmailVerificationMessage </summary>
    [XmlRoot("StartEmailVerificationMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class StartEmailVerificationCommand : MsmqMessage<StartEmailVerificationCommand>
    {
        public Guid AccountId { get; set; }
        public String Forename { get; set; }
        public String Email { get; set; }
        public Guid ChangeId { get; set; }
        public String UriFragment { get; set; }
        public Int32 TimeoutMinutes { get; set; }
    }
}
