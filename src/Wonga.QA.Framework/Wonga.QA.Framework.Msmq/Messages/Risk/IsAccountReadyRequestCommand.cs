using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IsAccountReadyRequest", Namespace = "Wonga.Risk", DataType = "")]
    public class IsAccountReadyRequestCommand : MsmqMessage<IsAccountReadyRequestCommand>
    {
        public Guid AccountId { get; set; }
    }
}
