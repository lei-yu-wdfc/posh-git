using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.IFixedTermApplicationAddedInternal </summary>
    [XmlRoot("IFixedTermApplicationAddedInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.IFixedTermApplicationAdded,Wonga.Payments.PublicMessages.IApplicationAdded,Wonga.Payments.PublicMessages.IPaymentsEvent" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IFixedTermApplicationAddedInternal : MsmqMessage<IFixedTermApplicationAddedInternal>
    {
        public DateTime? NextDueDate { get; set; }
        public DateTime PromiseDate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public String ApplicationReference { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
