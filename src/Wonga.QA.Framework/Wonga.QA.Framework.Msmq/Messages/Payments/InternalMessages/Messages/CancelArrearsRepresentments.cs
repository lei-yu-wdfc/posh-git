using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CancelArrearsRepresentments </summary>
    [XmlRoot("CancelArrearsRepresentments", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CancelArrearsRepresentments : MsmqMessage<CancelArrearsRepresentments>
    {
        public Guid ApplicationId { get; set; }
        public Guid ArrearsRepresentmentId { get; set; }
    }
}
