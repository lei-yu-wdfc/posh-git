using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SubscribeToAccountPreparedEvent </summary>
    [XmlRoot("SubscribeToAccountPreparedEvent", Namespace = "Wonga.Risk", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SubscribeToAccountPreparedEvent : MsmqMessage<SubscribeToAccountPreparedEvent>
    {
        public Guid AccountId { get; set; }
        public String SubscriberType { get; set; }
        public Guid SubscriberSagaId { get; set; }
    }
}
