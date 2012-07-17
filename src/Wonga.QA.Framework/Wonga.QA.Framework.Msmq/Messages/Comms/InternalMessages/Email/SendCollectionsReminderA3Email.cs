using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendCollectionsReminderA3Email </summary>
    [XmlRoot("SendCollectionsReminderA3Email", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.Email.SendCollectionsReminderBase,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendCollectionsReminderA3Email : MsmqMessage<SendCollectionsReminderA3Email>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}