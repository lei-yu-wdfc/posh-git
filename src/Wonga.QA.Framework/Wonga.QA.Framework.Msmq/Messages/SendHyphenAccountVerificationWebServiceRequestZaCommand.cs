using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.SendHyphenAccountVerificationWebServiceRequestMessage </summary>
    [XmlRoot("SendHyphenAccountVerificationWebServiceRequestMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za", DataType = "")]
    public partial class SendHyphenAccountVerificationWebServiceRequestZaCommand : MsmqMessage<SendHyphenAccountVerificationWebServiceRequestZaCommand>
    {
        public Int32 BankAccountVerificationId { get; set; }
    }
}
