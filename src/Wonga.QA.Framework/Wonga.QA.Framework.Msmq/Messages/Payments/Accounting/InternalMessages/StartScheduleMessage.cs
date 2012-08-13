using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Accounting.InternalMessages
{
    /// <summary> Wonga.Payments.Accounting.InternalMessages.StartScheduleMessage </summary>
    [XmlRoot("StartScheduleMessage", Namespace = "Wonga.Payments.Accounting.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Payments.Accounting.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class StartScheduleMessage : MsmqMessage<StartScheduleMessage>
    {
        public String ScheduleName { get; set; }
    }
}
