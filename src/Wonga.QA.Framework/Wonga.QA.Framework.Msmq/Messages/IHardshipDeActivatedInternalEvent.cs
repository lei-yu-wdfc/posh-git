using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.IHardshipDeActivatedInternal </summary>
    [XmlRoot("IHardshipDeActivatedInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.IHardshipDeactivated")]
    public partial class IHardshipDeActivatedInternalEvent : MsmqMessage<IHardshipDeActivatedInternalEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid CaseId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
