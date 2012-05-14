using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    [XmlRoot("IEventNewPremiumCardCommandDone", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "")]
    public partial class INewPremiumCardCommandDoneEvent : MsmqMessage<INewPremiumCardCommandDoneEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public String CardType { get; set; }
    }
}