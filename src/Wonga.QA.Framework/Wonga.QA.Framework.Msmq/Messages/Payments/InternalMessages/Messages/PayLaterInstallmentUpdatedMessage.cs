using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.PayLaterInstallmentUpdatedMessage </summary>
    [XmlRoot("PayLaterInstallmentUpdatedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class PayLaterInstallmentUpdatedMessage : MsmqMessage<PayLaterInstallmentUpdatedMessage>
    {
        public Int32 InstallmentId { get; set; }
        public Guid SagaId { get; set; }
    }
}
