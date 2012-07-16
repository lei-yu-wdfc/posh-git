using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.GetDeclineAdviceTextResponseMessage </summary>
    [XmlRoot("GetDeclineAdviceTextResponseMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class GetDeclineAdviceTextResponseMessage : MsmqMessage<GetDeclineAdviceTextResponseMessage>
    {
        public String Contents { get; set; }
        public Guid SagaId { get; set; }
    }
}
