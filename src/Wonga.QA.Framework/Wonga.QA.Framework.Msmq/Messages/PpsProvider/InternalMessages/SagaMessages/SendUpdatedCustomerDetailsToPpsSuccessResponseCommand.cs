using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.SendUpdatedCustomerDetailsToPpsSuccessMessageResponse </summary>
    [XmlRoot("SendUpdatedCustomerDetailsToPpsSuccessMessageResponse", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class SendUpdatedCustomerDetailsToPpsSuccessResponseCommand : MsmqMessage<SendUpdatedCustomerDetailsToPpsSuccessResponseCommand>
    {
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
    }
}
