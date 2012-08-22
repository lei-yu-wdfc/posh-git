using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.TransactionComposite
{
    /// <summary> Wonga.Risk.TransactionComposite.PaymentCategoryRequestMessage </summary>
    [XmlRoot("PaymentCategoryRequestMessage", Namespace = "Wonga.Risk.TransactionComposite", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class PaymentCategoryRequestMessage : MsmqMessage<PaymentCategoryRequestMessage>
    {
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
