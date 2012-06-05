using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.MarkAccountAsNotHardship </summary>
    [XmlRoot("MarkAccountAsNotHardship", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class MarkAccountAsNotHardshipCommand : MsmqMessage<MarkAccountAsNotHardshipCommand>
    {
        public Guid AccountId { get; set; }
    }
}
