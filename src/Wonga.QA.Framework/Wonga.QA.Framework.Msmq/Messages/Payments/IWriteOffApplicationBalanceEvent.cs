using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    /// <summary> Wonga.Payments.PublicMessages.IWriteOffApplicationBalance </summary>
    [XmlRoot("IWriteOffApplicationBalance", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IWriteOffApplicationBalanceEvent : MsmqMessage<IWriteOffApplicationBalanceEvent>
    {
        public Guid ApplicationId { get; set; }
        public String Reference { get; set; }
    }
}
