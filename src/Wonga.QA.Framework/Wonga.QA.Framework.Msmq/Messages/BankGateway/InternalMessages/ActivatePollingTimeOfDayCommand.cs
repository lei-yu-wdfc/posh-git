using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.ActivatePollingTimeOfDayMessage </summary>
    [XmlRoot("ActivatePollingTimeOfDayMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class ActivatePollingTimeOfDayCommand : MsmqMessage<ActivatePollingTimeOfDayCommand>
    {
        public String ScheduleName { get; set; }
        public List<String> PoolingDateTimes { get; set; }
    }
}
