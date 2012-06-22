using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.GetDeclineAdviceTextResponseMessage </summary>
    [XmlRoot("GetDeclineAdviceTextResponseMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class GetDeclineAdviceTextResponseCommand : MsmqMessage<GetDeclineAdviceTextResponseCommand>
    {
        public String Contents { get; set; }
        public Guid SagaId { get; set; }
    }
}
