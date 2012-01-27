using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IsAccountReadyResponse", Namespace = "Wonga.Risk", DataType = "")]
    public class IsAccountReadyResponseCommand : MsmqMessage<IsAccountReadyResponseCommand>
    {
        public Guid AccountId { get; set; }
        public Boolean IsAccountReady { get; set; }
    }
}
