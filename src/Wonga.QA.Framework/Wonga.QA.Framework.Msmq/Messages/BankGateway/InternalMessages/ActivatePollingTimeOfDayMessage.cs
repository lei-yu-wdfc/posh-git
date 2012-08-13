using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.ActivatePollingTimeOfDayMessage </summary>
    [XmlRoot("ActivatePollingTimeOfDayMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "Wonga.BankGateway.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ActivatePollingTimeOfDayMessage : MsmqMessage<ActivatePollingTimeOfDayMessage>
    {
        public Guid SagaId { get; set; }
    }
}
