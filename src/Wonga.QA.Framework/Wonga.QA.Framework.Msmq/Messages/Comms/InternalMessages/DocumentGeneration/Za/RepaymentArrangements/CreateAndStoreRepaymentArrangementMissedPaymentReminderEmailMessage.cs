using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateAndStoreRepaymentArrangementMissedPaymentReminderEmailMessage </summary>
    [XmlRoot("CreateAndStoreRepaymentArrangementMissedPaymentReminderEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.DocumentGeneration.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateAndStoreRepaymentArrangementMissedPaymentReminderEmailMessage : MsmqMessage<CreateAndStoreRepaymentArrangementMissedPaymentReminderEmailMessage>
    {
        public Guid RepaymentArrangementDetailId { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
