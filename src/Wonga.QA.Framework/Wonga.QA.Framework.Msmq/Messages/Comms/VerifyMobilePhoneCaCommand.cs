using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.Commands.Ca.VerifyMobilePhoneCaMessage </summary>
    [XmlRoot("VerifyMobilePhoneCaMessage", Namespace = "Wonga.Comms.Commands.Ca", DataType = "")]
    public partial class VerifyMobilePhoneCaCommand : MsmqMessage<VerifyMobilePhoneCaCommand>
    {
        public Guid VerificationId { get; set; }
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public String Forename { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
