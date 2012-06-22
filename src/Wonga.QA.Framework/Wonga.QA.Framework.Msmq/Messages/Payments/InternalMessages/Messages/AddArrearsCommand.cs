using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AddArrearsMessage </summary>
    [XmlRoot("AddArrearsMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class AddArrearsCommand : MsmqMessage<AddArrearsCommand>
    {
        public Int32 ApplicationId { get; set; }
        public PaymentTransactionEnum? PaymentTransactionType { get; set; }
        public Guid ReferenceId { get; set; }
    }
}
