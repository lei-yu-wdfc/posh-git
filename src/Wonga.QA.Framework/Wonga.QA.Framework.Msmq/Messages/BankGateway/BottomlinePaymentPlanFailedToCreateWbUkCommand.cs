using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("BottomlinePaymentPlanFailedToCreateMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk", DataType = "")]
    public class BottomlinePaymentPlanFailedToCreateWbUkCommand : MsmqMessage<BottomlinePaymentPlanFailedToCreateWbUkCommand>
    {
        public Int32 DirectDebitId { get; set; }
    }
}
