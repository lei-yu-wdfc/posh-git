using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages
{
    /// <summary> Wonga.Email.InternalMessages.SendExtendedArrearsCommunicationEmailMessage </summary>
    [XmlRoot("SendExtendedArrearsCommunicationEmailMessage", Namespace = "Wonga.Email.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Email.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendExtendedArrearsCommunicationEmailMessage : MsmqMessage<SendExtendedArrearsCommunicationEmailMessage>
    {
        public Guid ArrearsCommunicationId { get; set; }
        public Guid AccountId { get; set; }
        public String TemplateName { get; set; }
        public Decimal EffectiveBallance { get; set; }
        public String CustomerReference { get; set; }
        public String CustomerEmailAddress { get; set; }
        public String CustomerForename { get; set; }
        public Guid OriginatingSagaId { get; set; }
        public String CustomerLastName { get; set; }
        public String CustomerNationalNumber { get; set; }
        public DateTime SignedOn { get; set; }
        public Decimal Interest { get; set; }
    }
}
