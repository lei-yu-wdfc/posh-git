using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("CancelUserActionMessage", Namespace = "Wonga.Risk", DataType = "")]
    public class CancelUserActionCommand : MsmqMessage<CancelUserActionCommand>
    {
        public Guid UserActionId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
