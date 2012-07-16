using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IArrearsRepresentmentsCanceled </summary>
    [XmlRoot("IArrearsRepresentmentsCanceled", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IArrearsRepresentmentsCanceled : MsmqMessage<IArrearsRepresentmentsCanceled>
    {
        public Guid ApplicationId { get; set; }
        public Guid ArrearsRepaymentId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
