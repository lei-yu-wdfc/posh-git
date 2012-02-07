using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("VerifyMobilePhoneMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "")]
    public partial class VerifyMobilePhoneZaCommand : MsmqMessage<VerifyMobilePhoneZaCommand>
    {
        public Guid VerificationId { get; set; }
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public String Forename { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
