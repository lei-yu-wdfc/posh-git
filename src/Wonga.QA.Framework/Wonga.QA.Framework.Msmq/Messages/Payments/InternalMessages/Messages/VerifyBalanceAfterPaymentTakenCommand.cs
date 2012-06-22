using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.VerifyBalanceAfterPaymentTakenMessage </summary>
    [XmlRoot("VerifyBalanceAfterPaymentTakenMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class VerifyBalanceAfterPaymentTakenCommand : MsmqMessage<VerifyBalanceAfterPaymentTakenCommand>
    {
        public Guid ApplicationId { get; set; }
    }
}
