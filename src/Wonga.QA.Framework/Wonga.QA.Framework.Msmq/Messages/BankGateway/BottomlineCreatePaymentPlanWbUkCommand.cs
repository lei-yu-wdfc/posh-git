using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.BottomlineCreatePaymentPlanMessage </summary>
    [XmlRoot("BottomlineCreatePaymentPlanMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk", DataType = "")]
    public partial class BottomlineCreatePaymentPlanWbUkCommand : MsmqMessage<BottomlineCreatePaymentPlanWbUkCommand>
    {
        public Int32 DirectDebitId { get; set; }
    }
}
