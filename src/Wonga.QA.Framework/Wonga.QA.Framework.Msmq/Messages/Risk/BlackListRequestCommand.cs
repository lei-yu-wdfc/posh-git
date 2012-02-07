using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("BlackListRequestMessage", Namespace = "Wonga.Risk.BlackList", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class BlackListRequestCommand : MsmqMessage<BlackListRequestCommand>
    {
        public Guid ApplicationId { get; set; }
        public String EmployerName { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
