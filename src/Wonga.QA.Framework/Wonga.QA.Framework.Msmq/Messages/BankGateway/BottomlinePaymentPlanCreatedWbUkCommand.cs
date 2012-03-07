using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.SagaMessages.BottomlinePaymentPlanCreatedMessage </summary>
    [XmlRoot("BottomlinePaymentPlanCreatedMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.SagaMessages", DataType = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class BottomlinePaymentPlanCreatedWbUkCommand : MsmqMessage<BottomlinePaymentPlanCreatedWbUkCommand>
    {
        public Int32 DirectDebitId { get; set; }
        public Guid SagaId { get; set; }
    }
}
