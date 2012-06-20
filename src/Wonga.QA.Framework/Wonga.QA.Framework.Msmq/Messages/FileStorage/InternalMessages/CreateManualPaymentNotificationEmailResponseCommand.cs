using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateManualPaymentNotificationEmailResponseMessage </summary>
    [XmlRoot("CreateManualPaymentNotificationEmailResponseMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class CreateManualPaymentNotificationEmailResponseCommand : MsmqMessage<CreateManualPaymentNotificationEmailResponseCommand>
    {
        public Guid ManualPaymentNotificationId { get; set; }
        public String HtmlContent { get; set; }
        public String PlainContent { get; set; }
    }
}
