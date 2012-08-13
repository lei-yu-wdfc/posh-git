using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.ITopupFundsTransferred </summary>
    [XmlRoot("ITopupFundsTransferred", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent" )
    , SourceAssembly("Wonga.Payments.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ITopupFundsTransferred : MsmqMessage<ITopupFundsTransferred>
    {
        public Guid AccountId { get; set; }
        public Guid TopupId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
