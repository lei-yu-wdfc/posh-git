using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.SaveSuppressionMessage </summary>
    [XmlRoot("SaveSuppressionMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class SaveSuppressionCommand : MsmqMessage<SaveSuppressionCommand>
    {
        public Guid? AccountId { get; set; }
        public Guid? ApplicationId { get; set; }
        public DateTime? SuppressedOn { get; set; }
        public DateTime? SuppressedTo { get; set; }
    }
}
