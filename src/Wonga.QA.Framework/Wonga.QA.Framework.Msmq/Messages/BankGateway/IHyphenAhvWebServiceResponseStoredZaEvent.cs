using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("IHyphenAHVWebServiceResponseStored", Namespace = "Wonga.BankGateway.InternalMessages.Events.Hyphen.Za", DataType = "")]
    public partial class IHyphenAhvWebServiceResponseStoredZaEvent : MsmqMessage<IHyphenAhvWebServiceResponseStoredZaEvent>
    {
        public Int32 BankAccountVerificationId { get; set; }
        public Int32 BankAccountVerificationResponseId { get; set; }
    }
}
