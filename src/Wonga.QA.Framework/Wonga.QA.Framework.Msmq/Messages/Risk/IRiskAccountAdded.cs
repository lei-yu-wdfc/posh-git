using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IRiskAccountAdded </summary>
    [XmlRoot("IRiskAccountAdded", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IRiskAccountAdded : MsmqMessage<IRiskAccountAdded>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
