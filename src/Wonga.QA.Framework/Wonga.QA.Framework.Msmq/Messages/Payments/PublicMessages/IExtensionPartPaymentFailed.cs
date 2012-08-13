using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IExtensionPartPaymentFailed </summary>
    [XmlRoot("IExtensionPartPaymentFailed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent" )
    , SourceAssembly("Wonga.Payments.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IExtensionPartPaymentFailed : MsmqMessage<IExtensionPartPaymentFailed>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ExtensionId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
