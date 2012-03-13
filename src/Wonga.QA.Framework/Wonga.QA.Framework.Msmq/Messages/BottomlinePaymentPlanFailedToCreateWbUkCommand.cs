using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.BottomlinePaymentPlanFailedToCreateMessage </summary>
    [XmlRoot("BottomlinePaymentPlanFailedToCreateMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk", DataType = "")]
    public partial class BottomlinePaymentPlanFailedToCreateWbUkCommand : MsmqMessage<BottomlinePaymentPlanFailedToCreateWbUkCommand>
    {
        public Int32 DirectDebitId { get; set; }
    }
}
