using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.RegisterNewPrepaidCardHolder </summary>
    [XmlRoot("RegisterNewPrepaidCardHolder", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class RegisterNewPrepaidCardHolderCommand : MsmqMessage<RegisterNewPrepaidCardHolderCommand>
    {
        public Guid SagaId { get; set; }
        public Object CardHolderDetails { get; set; }
    }
}
