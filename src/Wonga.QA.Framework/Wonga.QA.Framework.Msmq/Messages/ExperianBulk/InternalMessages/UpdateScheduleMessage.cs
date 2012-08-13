using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.ExperianBulk.InternalMessages
{
    /// <summary> Wonga.ExperianBulk.InternalMessages.UpdateScheduleMessage </summary>
    [XmlRoot("UpdateScheduleMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.ExperianBulk.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdateScheduleMessage : MsmqMessage<UpdateScheduleMessage>
    {
        public String ScheduleName { get; set; }
        public List<DateTime> ReferenceDates { get; set; }
    }
}
