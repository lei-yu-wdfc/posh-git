using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("ProcessScheduledRepaymentForTimezone", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ProcessScheduledRepaymentForTimezoneCommand : MsmqMessage<ProcessScheduledRepaymentForTimezoneCommand>
    {
        public MsTimeZoneEnum TimeZone { get; set; }
        public Guid RequestId { get; set; }
    }
}
