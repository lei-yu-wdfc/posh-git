using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.IBusinessBankAccountActivated </summary>
    [XmlRoot("IBusinessBankAccountActivated", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class IBusinessBankAccountActivatedEvent : MsmqMessage<IBusinessBankAccountActivatedEvent>
    {
        public Guid BankAccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
