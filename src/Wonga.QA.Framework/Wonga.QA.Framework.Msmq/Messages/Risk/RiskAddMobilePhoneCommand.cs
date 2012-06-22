using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.RiskAddMobilePhone </summary>
    [XmlRoot("RiskAddMobilePhone", Namespace = "Wonga.Risk", DataType = "")]
    public partial class RiskAddMobilePhoneCommand : MsmqMessage<RiskAddMobilePhoneCommand>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
