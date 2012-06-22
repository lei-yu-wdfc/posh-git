using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.ICreditLimitChanged </summary>
    [XmlRoot("ICreditLimitChanged", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class ICreditLimitChangedEvent : MsmqMessage<ICreditLimitChangedEvent>
    {
        public Guid AccountId { get; set; }
        public Decimal CreditLimit { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
