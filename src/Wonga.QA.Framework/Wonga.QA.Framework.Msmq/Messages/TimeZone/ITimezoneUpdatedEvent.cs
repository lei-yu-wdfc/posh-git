using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.TimeZone
{
    /// <summary> Wonga.Timezone.PublicMessages.ITimezoneUpdated </summary>
    [XmlRoot("ITimezoneUpdated", Namespace = "Wonga.Timezone.PublicMessages", DataType = "")]
    public partial class ITimezoneUpdatedEvent : MsmqMessage<ITimezoneUpdatedEvent>
    {
        public Guid AccountId { get; set; }
        public MsTimeZoneEnum TimeZone { get; set; }
    }
}
