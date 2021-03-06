using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.TimeZone;

namespace Wonga.QA.Framework.Msmq.Messages.Timezone.PublicMessages
{
    /// <summary> Wonga.Timezone.PublicMessages.ITimezoneUpdated </summary>
    [XmlRoot("ITimezoneUpdated", Namespace = "Wonga.Timezone.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Timezone.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ITimezoneUpdated : MsmqMessage<ITimezoneUpdated>
    {
        public Guid AccountId { get; set; }
        public MsTimeZoneEnum TimeZone { get; set; }
    }
}
