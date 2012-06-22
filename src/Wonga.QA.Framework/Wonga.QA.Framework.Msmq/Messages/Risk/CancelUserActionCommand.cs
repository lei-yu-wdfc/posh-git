using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.CancelUserActionMessage </summary>
    [XmlRoot("CancelUserActionMessage", Namespace = "Wonga.Risk", DataType = "")]
    public partial class CancelUserActionCommand : MsmqMessage<CancelUserActionCommand>
    {
        public Guid UserActionId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
