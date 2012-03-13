using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.HyphenAccountVerificationResponseMessage </summary>
    [XmlRoot("HyphenAccountVerificationResponseMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class HyphenAccountVerificationResponseCommand : MsmqMessage<HyphenAccountVerificationResponseCommand>
    {
        public Int32 BankAccountVerificationId { get; set; }
    }
}
