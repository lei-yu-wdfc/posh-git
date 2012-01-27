using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("BottomlineCreatePaymentPlanMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk", DataType = "")]
    public class BottomlineCreatePaymentPlanWbUkCommand : MsmqMessage<BottomlineCreatePaymentPlanWbUkCommand>
    {
        public Int32 DirectDebitId { get; set; }
    }
}
