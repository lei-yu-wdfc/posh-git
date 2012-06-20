using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Ca
{
    /// <summary> Wonga.Comms.Commands.Ca.VerifyHomePhoneMessage </summary>
    [XmlRoot("VerifyHomePhoneMessage", Namespace = "Wonga.Comms.Commands.Ca", DataType = "")]
    public partial class VerifyHomePhoneCaCommand : MsmqMessage<VerifyHomePhoneCaCommand>
    {
        public Guid VerificationId { get; set; }
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
        public String Forename { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
