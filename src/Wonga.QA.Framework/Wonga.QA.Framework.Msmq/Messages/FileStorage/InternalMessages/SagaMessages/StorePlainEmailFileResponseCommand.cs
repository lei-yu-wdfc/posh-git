using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages.SagaMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.SagaMessages.StorePlainEmailFileResponseMessage </summary>
    [XmlRoot("StorePlainEmailFileResponseMessage", Namespace = "Wonga.FileStorage.InternalMessages.SagaMessages", DataType = "Wonga.FileStorage.InternalMessages.SagaMessages.BaseStoreEmailFileResponseMessage,Wonga.FileStorage.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class StorePlainEmailFileResponseCommand : MsmqMessage<StorePlainEmailFileResponseCommand>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
