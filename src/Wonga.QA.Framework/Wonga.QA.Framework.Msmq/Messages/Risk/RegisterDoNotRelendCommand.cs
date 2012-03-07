using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.UI.RegisterDoNotRelendMessage </summary>
    [XmlRoot("RegisterDoNotRelendMessage", Namespace = "Wonga.Risk.UI", DataType = "")]
    public partial class RegisterDoNotRelendCommand : MsmqMessage<RegisterDoNotRelendCommand>
    {
        public Guid AccountId { get; set; }
        public Boolean DoNotRelend { get; set; }
    }
}
