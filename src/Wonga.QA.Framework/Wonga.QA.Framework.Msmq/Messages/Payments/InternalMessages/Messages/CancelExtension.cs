using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CancelExtension </summary>
    [XmlRoot("CancelExtension", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CancelExtension : MsmqMessage<CancelExtension>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid AccountId { get; set; }
    }
}
