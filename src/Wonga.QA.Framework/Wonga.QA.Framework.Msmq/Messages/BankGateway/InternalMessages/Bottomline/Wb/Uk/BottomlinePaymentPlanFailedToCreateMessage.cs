using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Bottomline.Wb.Uk
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk.BottomlinePaymentPlanFailedToCreateMessage </summary>
    [XmlRoot("BottomlinePaymentPlanFailedToCreateMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Bottomline.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BottomlinePaymentPlanFailedToCreateMessage : MsmqMessage<BottomlinePaymentPlanFailedToCreateMessage>
    {
        public Int32 DirectDebitId { get; set; }
    }
}
