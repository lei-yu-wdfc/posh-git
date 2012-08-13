using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Ca
{
    /// <summary> Wonga.Comms.Commands.Ca.VerifyHomePhoneMessage </summary>
    [XmlRoot("VerifyHomePhoneMessage", Namespace = "Wonga.Comms.Commands.Ca", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class VerifyHomePhone : MsmqMessage<VerifyHomePhone>
    {
        public Guid VerificationId { get; set; }
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
        public String Forename { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
