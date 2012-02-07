using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SendSecciEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendSecciEmailCommand : MsmqMessage<SendSecciEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public Guid SecciFileId { get; set; }
        public String Forename { get; set; }
        public String TotalRepayable { get; set; }
        public String PageUrl { get; set; }
        public Guid SagaId { get; set; }
    }
}
