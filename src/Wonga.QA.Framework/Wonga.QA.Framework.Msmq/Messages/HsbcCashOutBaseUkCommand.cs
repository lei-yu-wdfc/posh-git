using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.HsbcCashOutBaseMessage </summary>
    [XmlRoot("HsbcCashOutBaseMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk", DataType = "")]
    public partial class HsbcCashOutBaseUkCommand : MsmqMessage<HsbcCashOutBaseUkCommand>
    {
        public Byte ServiceId { get; set; }
    }
}
