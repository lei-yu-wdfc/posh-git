using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("ChapsSecondResponseSuccessSagaMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages.BaseSecondResponseMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class ChapsSecondResponseSuccessSagaUkCommand : MsmqMessage<ChapsSecondResponseSuccessSagaUkCommand>
    {
        public String TransactionReference { get; set; }
        public String FileName { get; set; }
        public String RawContents { get; set; }
        public Guid SagaId { get; set; }
    }
}
