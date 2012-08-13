using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ExtendLoanStartedInternal </summary>
    [XmlRoot("ExtendLoanStartedInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ExtendLoanStartedInternal : MsmqMessage<ExtendLoanStartedInternal>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public DateTime ExtendDate { get; set; }
        public Decimal PartPaymentRequired { get; set; }
        public Decimal NewFinalBalance { get; set; }
    }
}
