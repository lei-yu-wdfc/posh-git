using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SubscribeToAccountPreparedEvent </summary>
    [XmlRoot("SubscribeToAccountPreparedEvent", Namespace = "Wonga.Risk", DataType = "")]
    public partial class SubscribeToAccountPreparedCommand : MsmqMessage<SubscribeToAccountPreparedCommand>
    {
        public Guid AccountId { get; set; }
        public String SubscriberType { get; set; }
        public Guid SubscriberSagaId { get; set; }
    }
}
