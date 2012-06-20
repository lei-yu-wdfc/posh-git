using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages
{
    /// <summary> Wonga.Payments.InternalMessages.WriteOffApplicationBalance </summary>
    [XmlRoot("WriteOffApplicationBalance", Namespace = "Wonga.Payments.InternalMessages", DataType = "")]
    public partial class WriteOffApplicationBalanceCommand : MsmqMessage<WriteOffApplicationBalanceCommand>
    {
        public Guid ApplicationId { get; set; }
        public String Reference { get; set; }
    }
}
