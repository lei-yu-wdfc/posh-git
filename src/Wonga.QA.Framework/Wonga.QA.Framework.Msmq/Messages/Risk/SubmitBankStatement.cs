using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SubmitBankStatementMessage </summary>
    [XmlRoot("SubmitBankStatementMessage", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.BaseHandleUserDataMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SubmitBankStatement : MsmqMessage<SubmitBankStatement>
    {
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
