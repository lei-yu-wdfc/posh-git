using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendCollectionsReminderA7Email </summary>
    [XmlRoot("SendCollectionsReminderA7Email", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.Email.SendCollectionsReminderBase,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendCollectionsReminderA7EmailCommand : MsmqMessage<SendCollectionsReminderA7EmailCommand>
    {
        public String LastName { get; set; }
        public String NationalNumber { get; set; }
        public DateTime SignedOn { get; set; }
        public Decimal Interest { get; set; }
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public Decimal Amount { get; set; }
        public String CustomerReference { get; set; }
        public Guid SagaId { get; set; }
    }
}
