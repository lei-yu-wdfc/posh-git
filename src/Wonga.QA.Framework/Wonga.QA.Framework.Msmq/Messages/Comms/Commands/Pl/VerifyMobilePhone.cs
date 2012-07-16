using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Pl
{
    /// <summary> Wonga.Comms.Commands.Pl.VerifyMobilePhonePlMessage </summary>
    [XmlRoot("VerifyMobilePhonePlMessage", Namespace = "Wonga.Comms.Commands.Pl", DataType = "")]
    public partial class VerifyMobilePhone : MsmqMessage<VerifyMobilePhone>
    {
        public Guid VerificationId { get; set; }
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public String Forename { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
