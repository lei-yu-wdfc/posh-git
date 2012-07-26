using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.BlackList
{
    /// <summary> Wonga.Risk.BlackList.BlackListRequestMessage </summary>
    [XmlRoot("BlackListRequestMessage", Namespace = "Wonga.Risk.BlackList", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class BlackListRequestMessage : MsmqMessage<BlackListRequestMessage>
    {
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
