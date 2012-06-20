using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments
{
    /// <summary> Wonga.PublicMessages.Payments.IBankAccountValidated </summary>
    [XmlRoot("IBankAccountValidated", Namespace = "Wonga.PublicMessages.Payments", DataType = "")]
    public partial class IBankAccountValidatedEvent : MsmqMessage<IBankAccountValidatedEvent>
    {
        public Boolean IsValid { get; set; }
        public List<String> Errors { get; set; }
        public Guid BankAccountId { get; set; }
    }
}
