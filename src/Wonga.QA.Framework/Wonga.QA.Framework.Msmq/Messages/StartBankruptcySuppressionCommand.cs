using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Suppressions.StartBankruptcySuppressionMessage </summary>
    [XmlRoot("StartBankruptcySuppressionMessage", Namespace = "Wonga.Comms.InternalMessages.Suppressions", DataType = "")]
    public partial class StartBankruptcySuppressionCommand : MsmqMessage<StartBankruptcySuppressionCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
