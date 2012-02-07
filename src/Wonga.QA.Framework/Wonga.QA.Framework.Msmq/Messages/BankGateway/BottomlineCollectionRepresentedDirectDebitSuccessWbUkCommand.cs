using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("BottomlineCollectionRepresentedDirectDebitSuccessMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk", DataType = "")]
    public partial class BottomlineCollectionRepresentedDirectDebitSuccessWbUkCommand : MsmqMessage<BottomlineCollectionRepresentedDirectDebitSuccessWbUkCommand>
    {
        public Int32 DirectDebitId { get; set; }
        public Int32 PaymentNumber { get; set; }
        public Decimal Amount { get; set; }
    }
}
