using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IOnlineBillPaymentCcinMismatched </summary>
    [XmlRoot("IOnlineBillPaymentCcinMismatched", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IOnlineBillPaymentFailed,Wonga.Payments.PublicMessages.IPaymentsEvent" )
    , SourceAssembly("Wonga.Payments.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IOnlineBillPaymentCcinMismatched : MsmqMessage<IOnlineBillPaymentCcinMismatched>
    {
        public Guid OnlineBillPaymentId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
