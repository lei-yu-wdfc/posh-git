using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.HSBC.Uk
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.HsbcCashOutStopMessage </summary>
    [XmlRoot("HsbcCashOutStopMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk", DataType = "")]
    public partial class HsbcCashOutStopUkCommand : MsmqMessage<HsbcCashOutStopUkCommand>
    {
        public Byte ServiceId { get; set; }
    }
}
