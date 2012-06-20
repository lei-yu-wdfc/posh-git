using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.ActivatePollingMessage </summary>
    [XmlRoot("ActivatePollingMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class ActivatePollingCommand : MsmqMessage<ActivatePollingCommand>
    {
        public String ScheduleName { get; set; }
        public TimeSpan Interval { get; set; }
    }
}
