using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("HyphenAccountVerificationResponseMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public class HyphenAccountVerificationResponseCommand : MsmqMessage<HyphenAccountVerificationResponseCommand>
    {
        public Int32 BankAccountVerificationId { get; set; }
    }
}
