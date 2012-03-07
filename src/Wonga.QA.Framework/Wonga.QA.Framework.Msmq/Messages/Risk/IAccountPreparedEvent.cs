using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.IAccountPrepared </summary>
    [XmlRoot("IAccountPrepared", Namespace = "Wonga.Risk", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IAccountPreparedEvent : MsmqMessage<IAccountPreparedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid PaymenCardId { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid SagaId { get; set; }
    }
}
