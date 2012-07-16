using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToTopUpPrepaidCard </summary>
    [XmlRoot("IWantToTopUpPrepaidCard", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "")]
    public partial class IWantToTopUpPrepaidCard : MsmqMessage<IWantToTopUpPrepaidCard>
    {
        public Guid SagaId { get; set; }
        public Guid CustomerExternalId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public String BankAccount { get; set; }
        public String BankCode { get; set; }
        public String BankAccountType { get; set; }
        public Guid SenderRefference { get; set; }
    }
}
