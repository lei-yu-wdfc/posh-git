using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Bottomline.Wb.Uk.SagaMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.SagaMessages.BottomlinePaymentPlanCreatedMessage </summary>
    [XmlRoot("BottomlinePaymentPlanCreatedMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.SagaMessages", DataType = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BottomlinePaymentPlanCreatedMessage : MsmqMessage<BottomlinePaymentPlanCreatedMessage>
    {
        public Int32 DirectDebitId { get; set; }
        public Guid SagaId { get; set; }
    }
}
