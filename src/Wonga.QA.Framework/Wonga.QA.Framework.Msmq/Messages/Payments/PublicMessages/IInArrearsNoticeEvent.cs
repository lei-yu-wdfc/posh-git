using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IInArrearsNotice </summary>
    [XmlRoot("IInArrearsNotice", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IInArrearsNoticeEvent : MsmqMessage<IInArrearsNoticeEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Int32 DaysInArrears { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
