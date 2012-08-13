using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Ca
{
    /// <summary> Wonga.Comms.Commands.Ca.VerifyMobilePhoneCaMessage </summary>
    [XmlRoot("VerifyMobilePhoneCaMessage", Namespace = "Wonga.Comms.Commands.Ca", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
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
