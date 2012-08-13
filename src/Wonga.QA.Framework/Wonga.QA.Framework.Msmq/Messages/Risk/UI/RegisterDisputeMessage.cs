using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.UI
{
    /// <summary> Wonga.Risk.UI.RegisterDisputeMessage </summary>
    [XmlRoot("RegisterDisputeMessage", Namespace = "Wonga.Risk.UI", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RegisterDisputeMessage : MsmqMessage<RegisterDisputeMessage>
    {
        public Guid AccountId { get; set; }
        public Boolean HasDispute { get; set; }
    }
}
