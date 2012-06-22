using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.SendVerificationEmail </summary>
    [XmlRoot("SendVerificationEmail", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class SendVerificationEmailCommand : MsmqMessage<SendVerificationEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String UriFragment { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
