using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Bottomline.Wb.Uk.SagaMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.SagaMessages.BaseSagaMessage </summary>
    [XmlRoot("BaseSagaMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BaseSagaMessage : MsmqMessage<BaseSagaMessage>
    {
        public Guid SagaId { get; set; }
    }
}
