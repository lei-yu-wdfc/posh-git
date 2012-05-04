using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.FileStorage.InternalMessages.SagaMessages.StoreHtmlEmailFileResponseMessage </summary>
    [XmlRoot("StoreHtmlEmailFileResponseMessage", Namespace = "Wonga.FileStorage.InternalMessages.SagaMessages", DataType = "Wonga.FileStorage.InternalMessages.SagaMessages.BaseStoreEmailFileResponseMessage,Wonga.FileStorage.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class StoreHtmlEmailFileResponseCommand : MsmqMessage<StoreHtmlEmailFileResponseCommand>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
