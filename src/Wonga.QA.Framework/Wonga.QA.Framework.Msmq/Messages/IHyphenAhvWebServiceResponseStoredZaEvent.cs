using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Events.Hyphen.Za.IHyphenAHVWebServiceResponseStored </summary>
    [XmlRoot("IHyphenAHVWebServiceResponseStored", Namespace = "Wonga.BankGateway.InternalMessages.Events.Hyphen.Za", DataType = "")]
    public partial class IHyphenAhvWebServiceResponseStoredZaEvent : MsmqMessage<IHyphenAhvWebServiceResponseStoredZaEvent>
    {
        public Int32 BankAccountVerificationId { get; set; }
        public Int32 BankAccountVerificationResponseId { get; set; }
    }
}
