using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.MarkAccountAsNotHardship </summary>
    [XmlRoot("MarkAccountAsNotHardship", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class MarkAccountAsNotHardship : MsmqMessage<MarkAccountAsNotHardship>
    {
        public Guid AccountId { get; set; }
    }
}
