using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bmo.Ca.ActivatePollingMessage </summary>
    [XmlRoot("ActivatePollingMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bmo.Ca", DataType = "")]
    public partial class ActivatePollingCaCommand : MsmqMessage<ActivatePollingCaCommand>
    {
        public String ScheduleName { get; set; }
    }
}
