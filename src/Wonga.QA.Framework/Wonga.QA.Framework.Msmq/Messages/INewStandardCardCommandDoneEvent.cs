using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    [XmlRoot("IEventNewStandartCardCommandDone", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "")]
    public class INewStandardCardCommandDoneEvent : MsmqMessage<INewStandardCardCommandDoneEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public String CardType { get; set; }
    }
}