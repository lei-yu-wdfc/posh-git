using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.InternalMessages.Events;

namespace Wonga.QA.Framework.Msmq.Messages.Timezone.PublicMessages
{
    /// <summary> Wonga.Timezone.PublicMessages.ITimezoneUpdated </summary>
    [XmlRoot("ITimezoneUpdated", Namespace = "Wonga.Timezone.PublicMessages", DataType = "")]
    public partial class ITimezoneUpdatedEvent : MsmqMessage<ITimezoneUpdatedEvent>
    {
        public Guid AccountId { get; set; }
        public MsTimeZoneEnum TimeZone { get; set; }
    }
}
