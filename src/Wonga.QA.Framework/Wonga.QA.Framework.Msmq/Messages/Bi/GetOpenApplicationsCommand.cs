using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Bi
{
    /// <summary> Wonga.Bi.Messages.GetOpenApplicationsMessage </summary>
    [XmlRoot("GetOpenApplicationsMessage", Namespace = "Wonga.Bi.Messages", DataType = "")]
    public partial class GetOpenApplicationsCommand : MsmqMessage<GetOpenApplicationsCommand>
    {
        public DateTime? OpenDate { get; set; }
    }
}
