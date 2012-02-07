using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Bi
{
    [XmlRoot("GetOpenApplicationsMessage", Namespace = "Wonga.Bi.Messages", DataType = "")]
    public partial class GetOpenApplicationsCommand : MsmqMessage<GetOpenApplicationsCommand>
    {
        public DateTime? OpenDate { get; set; }
    }
}
