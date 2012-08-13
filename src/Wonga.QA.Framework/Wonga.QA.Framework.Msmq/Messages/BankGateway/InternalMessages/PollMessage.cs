using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.PollMessage </summary>
    [XmlRoot("PollMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class PollMessage : MsmqMessage<PollMessage>
    {
        public String ScheduleName { get; set; }
        public Boolean ForcePollEvenIfInactive { get; set; }
    }
}
