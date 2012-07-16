using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.TimeZone;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ProcessScheduledPaymentForTimezone </summary>
    [XmlRoot("ProcessScheduledPaymentForTimezone", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ProcessScheduledPaymentForTimezone : MsmqMessage<ProcessScheduledPaymentForTimezone>
    {
        public MsTimeZoneEnum TimeZone { get; set; }
        public Guid RequestId { get; set; }
    }
}
