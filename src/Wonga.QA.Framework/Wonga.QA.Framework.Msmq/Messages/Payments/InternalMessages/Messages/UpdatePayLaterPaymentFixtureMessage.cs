using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.UpdatePayLaterPaymentFixtureMessage </summary>
    [XmlRoot("UpdatePayLaterPaymentFixtureMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class UpdatePayLaterPaymentFixtureMessage : MsmqMessage<UpdatePayLaterPaymentFixtureMessage>
    {
        public Int32 FixtureId { get; set; }
        public Guid SagaId { get; set; }
        public PayLaterPaymentFixtureStatusEnum? Status { get; set; }
        public Decimal? DiscardValue { get; set; }
        public Decimal? PenaltyValue { get; set; }
        public Decimal? PaidAmount { get; set; }
    }
}
