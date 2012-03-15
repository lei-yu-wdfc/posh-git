using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Events.Hyphen.Za.IHyphenAHVWebServiceCallFailed </summary>
    [XmlRoot("IHyphenAHVWebServiceCallFailed", Namespace = "Wonga.BankGateway.InternalMessages.Events.Hyphen.Za", DataType = "")]
    public partial class IHyphenAhvWebServiceCallFailedZaEvent : MsmqMessage<IHyphenAhvWebServiceCallFailedZaEvent>
    {
        public Int32 BankAccountVerificationId { get; set; }
    }
}
