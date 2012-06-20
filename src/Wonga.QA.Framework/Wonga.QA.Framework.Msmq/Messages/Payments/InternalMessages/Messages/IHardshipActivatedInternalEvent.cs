using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.IHardshipActivatedInternal </summary>
    [XmlRoot("IHardshipActivatedInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.IHardshipActivated")]
    public partial class IHardshipActivatedInternalEvent : MsmqMessage<IHardshipActivatedInternalEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid CaseId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
