using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Suppressions.StopBankruptcySuppressionMessage </summary>
    [XmlRoot("StopBankruptcySuppressionMessage", Namespace = "Wonga.Comms.InternalMessages.Suppressions", DataType = "")]
    public partial class StopBankruptcySuppressionCommand : MsmqMessage<StopBankruptcySuppressionCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
