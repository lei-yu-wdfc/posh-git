using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Bi.Messages
{
    /// <summary> Wonga.Bi.Messages.GetOpenApplicationsMessage </summary>
    [XmlRoot("GetOpenApplicationsMessage", Namespace = "Wonga.Bi.Messages", DataType = "")]
    public partial class GetOpenApplicationsMessage : MsmqMessage<GetOpenApplicationsMessage>
    {
        public DateTime? OpenDate { get; set; }
    }
}
