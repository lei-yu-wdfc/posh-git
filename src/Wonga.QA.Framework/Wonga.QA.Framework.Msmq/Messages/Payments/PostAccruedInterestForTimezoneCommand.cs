using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("PostAccruedInterestForTimezone", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class PostAccruedInterestForTimezoneCommand : MsmqMessage<PostAccruedInterestForTimezoneCommand>
    {
        public MsTimeZoneEnum TimeZone { get; set; }
    }
}
