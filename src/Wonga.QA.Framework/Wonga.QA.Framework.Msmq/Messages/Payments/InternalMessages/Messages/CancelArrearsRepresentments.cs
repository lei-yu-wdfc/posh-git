using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CancelArrearsRepresentments </summary>
    [XmlRoot("CancelArrearsRepresentments", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CancelArrearsRepresentments : MsmqMessage<CancelArrearsRepresentments>
    {
        public Guid ApplicationId { get; set; }
        public Guid ArrearsRepresentmentId { get; set; }
    }
}
