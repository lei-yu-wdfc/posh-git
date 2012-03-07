using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.PostAccruedInterestForTimezone </summary>
    [XmlRoot("PostAccruedInterestForTimezone", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class PostAccruedInterestForTimezoneCommand : MsmqMessage<PostAccruedInterestForTimezoneCommand>
    {
        public MsTimeZoneEnum TimeZone { get; set; }
    }
}
