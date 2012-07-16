using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.HSBC.Uk
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.HsbcCashOutStartMessage </summary>
    [XmlRoot("HsbcCashOutStartMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk", DataType = "")]
    public partial class HsbcCashOutStartMessage : MsmqMessage<HsbcCashOutStartMessage>
    {
        public Byte ServiceId { get; set; }
    }
}
