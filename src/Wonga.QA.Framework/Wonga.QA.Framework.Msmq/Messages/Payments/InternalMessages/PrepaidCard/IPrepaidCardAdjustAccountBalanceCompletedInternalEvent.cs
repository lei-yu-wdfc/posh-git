using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.PrepaidCard
{
    /// <summary> Wonga.Payments.InternalMessages.PrepaidCard.IPrepaidCardAdjustAccountBalanceCompletedInternalEvent </summary>
    [XmlRoot("IPrepaidCardAdjustAccountBalanceCompletedInternalEvent", Namespace = "Wonga.Payments.InternalMessages.PrepaidCard", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages.PrepaidCard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IPrepaidCardAdjustAccountBalanceCompletedInternalEvent : MsmqMessage<IPrepaidCardAdjustAccountBalanceCompletedInternalEvent>
    {
        public Guid SagaId { get; set; }
        public DateTime? SucceededOn { get; set; }
        public DateTime? FailedOn { get; set; }
        public String PpsLocalTimeStamp { get; set; }
        public Guid? TransactionId { get; set; }
        public String AdditionalInformation { get; set; }
    }
}
