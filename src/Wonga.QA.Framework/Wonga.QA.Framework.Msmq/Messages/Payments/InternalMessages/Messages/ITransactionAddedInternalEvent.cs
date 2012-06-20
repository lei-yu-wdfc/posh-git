using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.Csapi.Commands;
using Wonga.QA.Framework.Msmq.Enums.FileStorage.InternalMessages;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ITransactionAddedInternal </summary>
    [XmlRoot("ITransactionAddedInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.ITransactionAdded,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class ITransactionAddedInternalEvent : MsmqMessage<ITransactionAddedInternalEvent>
    {
        public Int32 ApplicationInternalId { get; set; }
        public PaymentTransactionEnum TransactionType { get; set; }
        public PaymentTransactionScopeEnum TransactionScope { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
