using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.UpdatePayLaterInstallmentMessage </summary>
    [XmlRoot("UpdatePayLaterInstallmentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdatePayLaterInstallmentMessage : MsmqMessage<UpdatePayLaterInstallmentMessage>
    {
        public Int32 InstallmentId { get; set; }
        public Guid SagaId { get; set; }
        public PayLaterInstallmentStatusEnum? Status { get; set; }
        public Decimal? DiscardValue { get; set; }
        public Decimal? PenaltyValue { get; set; }
        public Decimal? PaidAmount { get; set; }
    }
}
