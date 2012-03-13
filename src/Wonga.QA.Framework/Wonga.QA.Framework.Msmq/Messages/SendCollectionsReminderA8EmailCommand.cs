using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendCollectionsReminderA8Email </summary>
    [XmlRoot("SendCollectionsReminderA8Email", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.Email.SendCollectionsReminderBase,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendCollectionsReminderA8EmailCommand : MsmqMessage<SendCollectionsReminderA8EmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public Decimal Amount { get; set; }
        public String CustomerReference { get; set; }
        public Guid SagaId { get; set; }
    }
}
