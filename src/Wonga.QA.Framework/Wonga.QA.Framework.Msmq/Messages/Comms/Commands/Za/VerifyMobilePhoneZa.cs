using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Za
{
    /// <summary> Wonga.Comms.Commands.Za.VerifyMobilePhoneMessage </summary>
    [XmlRoot("VerifyMobilePhoneMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class VerifyMobilePhoneZa : MsmqMessage<VerifyMobilePhoneZa>
    {
        public Guid VerificationId { get; set; }
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public String Forename { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
