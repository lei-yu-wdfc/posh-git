using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.TimeZone;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.PostAccruedInterestForTimezone </summary>
    [XmlRoot("PostAccruedInterestForTimezone", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class PostAccruedInterestForTimezone : MsmqMessage<PostAccruedInterestForTimezone>
    {
        public MsTimeZoneEnum TimeZone { get; set; }
    }
}
