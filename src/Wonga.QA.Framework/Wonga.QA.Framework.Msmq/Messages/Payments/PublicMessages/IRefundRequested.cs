using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IRefundRequested </summary>
    [XmlRoot("IRefundRequested", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent" )
    , SourceAssembly("Wonga.Payments.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRefundRequested : MsmqMessage<IRefundRequested>
    {
        public Guid ApplicaitonId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
