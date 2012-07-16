using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.GetDeclineAdviceTextMessage </summary>
    [XmlRoot("GetDeclineAdviceTextMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class GetDeclineAdviceTextMessage : MsmqMessage<GetDeclineAdviceTextMessage>
    {
        public Guid OriginatingSagaId { get; set; }
        public String DeclineAdviceUrl { get; set; }
    }
}
