using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.UI.RegisterFraudMessage </summary>
    [XmlRoot("RegisterFraudMessage", Namespace = "Wonga.Risk.UI", DataType = "")]
    public partial class RegisterFraudCommand : MsmqMessage<RegisterFraudCommand>
    {
        public Guid AccountId { get; set; }
        public Boolean HasFraud { get; set; }
    }
}
