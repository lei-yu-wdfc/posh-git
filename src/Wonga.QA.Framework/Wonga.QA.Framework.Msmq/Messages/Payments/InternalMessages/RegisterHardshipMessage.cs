using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages
{
    /// <summary> Wonga.Payments.InternalMessages.RegisterHardshipMessage </summary>
    [XmlRoot("RegisterHardshipMessage", Namespace = "Wonga.Payments.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RegisterHardshipMessage : MsmqMessage<RegisterHardshipMessage>
    {
        public Guid AccountId { get; set; }
        public Boolean HasHardship { get; set; }
    }
}
