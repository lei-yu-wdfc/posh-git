using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Bi.Messages.CalcAccruedInterestMessage </summary>
    [XmlRoot("CalcAccruedInterestMessage", Namespace = "Wonga.Bi.Messages", DataType = "")]
    public partial class CalcAccruedInterestCommand : MsmqMessage<CalcAccruedInterestCommand>
    {
        public Int32 ApplicationId { get; set; }
        public Guid ApplicationGuid { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
