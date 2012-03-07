using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.IRiskEvent </summary>
    [XmlRoot("IRiskEvent", Namespace = "Wonga.Risk", DataType = "")]
    public partial class IRiskEvent : MsmqMessage<IRiskEvent>
    {
        public DateTime CreatedOn { get; set; }
    }
}
