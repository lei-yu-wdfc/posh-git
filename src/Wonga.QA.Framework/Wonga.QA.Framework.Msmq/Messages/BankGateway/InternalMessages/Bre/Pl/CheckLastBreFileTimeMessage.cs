using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Bre.Pl
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bre.Pl.CheckLastBreFileTimeMessage </summary>
    [XmlRoot("CheckLastBreFileTimeMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bre.Pl", DataType = "")]
    public partial class CheckLastBreFileTimeMessage : MsmqMessage<CheckLastBreFileTimeMessage>
    {
    }
}
