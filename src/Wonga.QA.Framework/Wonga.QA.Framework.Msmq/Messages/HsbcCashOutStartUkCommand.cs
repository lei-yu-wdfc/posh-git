using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.HsbcCashOutStartMessage </summary>
    [XmlRoot("HsbcCashOutStartMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk", DataType = "")]
    public partial class HsbcCashOutStartUkCommand : MsmqMessage<HsbcCashOutStartUkCommand>
    {
        public Byte ServiceId { get; set; }
    }
}
