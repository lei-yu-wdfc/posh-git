using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("RegisterDisputeMessage", Namespace = "Wonga.Risk.UI", DataType = "")]
    public partial class RegisterDisputeCommand : MsmqMessage<RegisterDisputeCommand>
    {
        public Guid AccountId { get; set; }
        public Boolean HasDispute { get; set; }
    }
}
