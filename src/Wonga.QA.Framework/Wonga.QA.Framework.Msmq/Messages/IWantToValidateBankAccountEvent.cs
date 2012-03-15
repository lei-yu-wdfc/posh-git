using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Payments.IWantToValidateBankAccount </summary>
    [XmlRoot("IWantToValidateBankAccount", Namespace = "Wonga.PublicMessages.Payments", DataType = "")]
    public partial class IWantToValidateBankAccountEvent : MsmqMessage<IWantToValidateBankAccountEvent>
    {
        public Guid BankAccountId { get; set; }
    }
}
