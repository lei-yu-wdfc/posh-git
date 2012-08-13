using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ExtendLoanInternal </summary>
    [XmlRoot("ExtendLoanInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ExtendLoanInternal : MsmqMessage<ExtendLoanInternal>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid? CashEntityId { get; set; }
        public DateTime NextDueDate { get; set; }
        public Guid RepaymentRequestId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
