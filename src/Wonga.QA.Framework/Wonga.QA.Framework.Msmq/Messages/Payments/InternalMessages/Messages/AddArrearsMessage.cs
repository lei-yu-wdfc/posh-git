using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AddArrearsMessage </summary>
    [XmlRoot("AddArrearsMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AddArrearsMessage : MsmqMessage<AddArrearsMessage>
    {
        public Int32 ApplicationId { get; set; }
        public PaymentTransactionEnum? PaymentTransactionType { get; set; }
        public Guid ReferenceId { get; set; }
    }
}
