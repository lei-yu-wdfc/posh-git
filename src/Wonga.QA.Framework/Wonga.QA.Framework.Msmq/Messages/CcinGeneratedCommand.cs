using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CcinGenerated </summary>
    [XmlRoot("CcinGenerated", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.ICcinGenerated,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class CcinGeneratedCommand : MsmqMessage<CcinGeneratedCommand>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public String Ccin { get; set; }
    }
}
