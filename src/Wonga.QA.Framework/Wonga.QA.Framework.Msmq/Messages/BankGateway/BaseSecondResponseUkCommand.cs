using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("BaseSecondResponseMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class BaseSecondResponseUkCommand : MsmqMessage<BaseSecondResponseUkCommand>
    {
        public String TransactionReference { get; set; }
        public String FileName { get; set; }
        public String RawContents { get; set; }
        public Guid SagaId { get; set; }
    }
}
