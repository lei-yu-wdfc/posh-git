using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.ISocialDetailsUpdated </summary>
    [XmlRoot("ISocialDetailsUpdated", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class ISocialDetailsUpdated : MsmqMessage<ISocialDetailsUpdated>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
