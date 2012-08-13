using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IAccountPrepared </summary>
    [XmlRoot("IAccountPrepared", Namespace = "Wonga.Risk", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IAccountPrepared : MsmqMessage<IAccountPrepared>
    {
        public Guid AccountId { get; set; }
        public Guid PaymenCardId { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid SagaId { get; set; }
    }
}
