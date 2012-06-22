using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ExtendFixedTermLoanProcessPaymentTakenMessage </summary>
    [XmlRoot("ExtendFixedTermLoanProcessPaymentTakenMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ExtendFixedTermLoanProcessPaymentTakenCommand : MsmqMessage<ExtendFixedTermLoanProcessPaymentTakenCommand>
    {
        public Guid ApplicationId { get; set; }
        public Int32 LoanExtensionId { get; set; }
        public Guid LoanExtensionExternalId { get; set; }
        public DateTime PaymentDate { get; set; }
        public Decimal ExtendAmount { get; set; }
    }
}
