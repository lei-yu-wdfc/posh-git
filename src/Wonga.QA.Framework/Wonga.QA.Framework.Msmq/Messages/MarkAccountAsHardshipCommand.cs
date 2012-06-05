using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.MarkAccountAsHardship </summary>
    [XmlRoot("MarkAccountAsHardship", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class MarkAccountAsHardshipCommand : MsmqMessage<MarkAccountAsHardshipCommand>
    {
        public Guid AccountId { get; set; }
    }
}
