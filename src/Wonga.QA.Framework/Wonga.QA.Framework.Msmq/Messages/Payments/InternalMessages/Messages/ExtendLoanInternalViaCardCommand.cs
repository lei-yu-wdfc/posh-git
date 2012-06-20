using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ExtendLoanInternalViaCard </summary>
    [XmlRoot("ExtendLoanInternalViaCard", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ExtendLoanInternalViaCardCommand : MsmqMessage<ExtendLoanInternalViaCardCommand>
    {
        public Object PaymentCardCv2 { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid? CashEntityId { get; set; }
        public DateTime NextDueDate { get; set; }
        public Guid RepaymentRequestId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
