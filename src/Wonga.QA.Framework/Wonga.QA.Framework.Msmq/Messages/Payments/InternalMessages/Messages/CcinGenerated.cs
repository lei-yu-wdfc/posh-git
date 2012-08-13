using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CcinGenerated </summary>
    [XmlRoot("CcinGenerated", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.ICcinGenerated,Wonga.Payments.PublicMessages.IPaymentsEvent" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CcinGenerated : MsmqMessage<CcinGenerated>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public String Ccin { get; set; }
    }
}
