using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Bi.Messages
{
    /// <summary> Wonga.Bi.Messages.CalcAccruedInterestMessage </summary>
    [XmlRoot("CalcAccruedInterestMessage", Namespace = "Wonga.Bi.Messages", DataType = "")]
    public partial class CalcAccruedInterestMessage : MsmqMessage<CalcAccruedInterestMessage>
    {
        public Int32 ApplicationId { get; set; }
        public Guid ApplicationGuid { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
