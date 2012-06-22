using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages
{
    /// <summary> Wonga.Email.InternalMessages.SendArrearsCommunicationEmailMessage </summary>
    [XmlRoot("SendArrearsCommunicationEmailMessage", Namespace = "Wonga.Email.InternalMessages", DataType = "")]
    public partial class SendArrearsCommunicationEmailCommand : MsmqMessage<SendArrearsCommunicationEmailCommand>
    {
        public Guid ArrearsCommunicationId { get; set; }
        public Guid AccountId { get; set; }
        public String TemplateName { get; set; }
        public Decimal EffectiveBallance { get; set; }
        public String CustomerReference { get; set; }
        public String CustomerEmailAddress { get; set; }
        public String CustomerForename { get; set; }
        public String CustomerLastname { get; set; }
        public String Street { get; set; }
        public String Town { get; set; }
        public String PostCode { get; set; }
        public Guid OriginatingSagaId { get; set; }
        public DateTime NextDueDate { get; set; }
    }
}
