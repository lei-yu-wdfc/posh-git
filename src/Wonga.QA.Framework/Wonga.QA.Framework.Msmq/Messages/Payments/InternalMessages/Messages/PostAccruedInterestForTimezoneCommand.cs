using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.InternalMessages.Events;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.PostAccruedInterestForTimezone </summary>
    [XmlRoot("PostAccruedInterestForTimezone", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class PostAccruedInterestForTimezoneCommand : MsmqMessage<PostAccruedInterestForTimezoneCommand>
    {
        public MsTimeZoneEnum TimeZone { get; set; }
    }
}
