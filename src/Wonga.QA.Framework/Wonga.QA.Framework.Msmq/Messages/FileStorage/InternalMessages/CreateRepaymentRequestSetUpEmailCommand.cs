using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateRepaymentRequestSetUpEmailMessage </summary>
    [XmlRoot("CreateRepaymentRequestSetUpEmailMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class CreateRepaymentRequestSetUpEmailCommand : MsmqMessage<CreateRepaymentRequestSetUpEmailCommand>
    {
        public Guid RepaymentRequestId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
