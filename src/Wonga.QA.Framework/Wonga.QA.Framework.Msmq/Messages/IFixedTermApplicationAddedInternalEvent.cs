using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.IFixedTermApplicationAddedInternal </summary>
    [XmlRoot("IFixedTermApplicationAddedInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.IFixedTermApplicationAdded,Wonga.Payments.PublicMessages.IApplicationAdded,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IFixedTermApplicationAddedInternalEvent : MsmqMessage<IFixedTermApplicationAddedInternalEvent>
    {
        public DateTime PromiseDate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public String ApplicationReference { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
