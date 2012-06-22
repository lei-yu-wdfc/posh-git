using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.PayLaterPaymentFixtureUpdatedMessage </summary>
    [XmlRoot("PayLaterPaymentFixtureUpdatedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class PayLaterPaymentFixtureUpdatedCommand : MsmqMessage<PayLaterPaymentFixtureUpdatedCommand>
    {
        public Int32 FixtureId { get; set; }
        public Guid SagaId { get; set; }
    }
}
