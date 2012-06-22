using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Handlers.Pingbot.Uk.Data
{
    /// <summary> Wonga.Payments.Handlers.Pingbot.Uk.Data.StartCustomerPingMessage </summary>
    [XmlRoot("StartCustomerPingMessage", Namespace = "Wonga.Payments.Handlers.Pingbot.Uk.Data", DataType = "")]
    public partial class StartCustomerPingUkCommand : MsmqMessage<StartCustomerPingUkCommand>
    {
        public Guid ApplicationId { get; set; }
        public Decimal AmountDue { get; set; }
    }
}
