using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IHsbcCashOutPpsStartMessage </summary>
    [XmlRoot("IHsbcCashOutPpsStartMessage", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IHsbcCashOutPpsStartMessage : MsmqMessage<IHsbcCashOutPpsStartMessage>
    {
    }
}
