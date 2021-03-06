using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Events.Hyphen.Za
{
    /// <summary> Wonga.BankGateway.InternalMessages.Events.Hyphen.Za.IHyphenAHVWebServiceCallFailed </summary>
    [XmlRoot("IHyphenAHVWebServiceCallFailed", Namespace = "Wonga.BankGateway.InternalMessages.Events.Hyphen.Za", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Events.Hyphen.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IHyphenAHVWebServiceCallFailed : MsmqMessage<IHyphenAHVWebServiceCallFailed>
    {
        public Int32 BankAccountVerificationId { get; set; }
    }
}
