using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Bottomline.Wb.Uk
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.BottomlineCollectionFirstDirectDebitSuccessMessage </summary>
    [XmlRoot("BottomlineCollectionFirstDirectDebitSuccessMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk", DataType = "")]
    public partial class BottomlineCollectionFirstDirectDebitSuccessMessage : MsmqMessage<BottomlineCollectionFirstDirectDebitSuccessMessage>
    {
        public Int32 DirectDebitId { get; set; }
        public Int32 PaymentNumber { get; set; }
        public Decimal Amount { get; set; }
    }
}
