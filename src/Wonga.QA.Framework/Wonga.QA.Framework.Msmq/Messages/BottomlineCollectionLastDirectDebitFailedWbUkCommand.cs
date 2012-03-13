using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.BottomlineCollectionLastDirectDebitFailedMessage </summary>
    [XmlRoot("BottomlineCollectionLastDirectDebitFailedMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk", DataType = "")]
    public partial class BottomlineCollectionLastDirectDebitFailedWbUkCommand : MsmqMessage<BottomlineCollectionLastDirectDebitFailedWbUkCommand>
    {
        public Int32 DirectDebitId { get; set; }
        public Int32 PaymentNumber { get; set; }
        public Decimal Amount { get; set; }
    }
}
