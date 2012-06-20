using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IHardshipReceived </summary>
    [XmlRoot("IHardshipReceived", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IHardshipReceivedEvent : MsmqMessage<IHardshipReceivedEvent>
    {
        public Guid AccountId { get; set; }
        public Boolean HasHardship { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
