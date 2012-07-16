using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Sms.Mblox.Data;

namespace Wonga.QA.Framework.Msmq.Messages.Sms.Mblox.InternalMessages
{
    /// <summary> Wonga.Sms.Mblox.InternalMessages.MbloxHttpCallbackNotificationMessage </summary>
    [XmlRoot("MbloxHttpCallbackNotificationMessage", Namespace = "Wonga.Sms.Mblox.InternalMessages", DataType = "Wonga.Sms.Mblox.InternalMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class MbloxHttpCallbackNotificationMessage : MsmqMessage<MbloxHttpCallbackNotificationMessage>
    {
        public Int32 SmsId { get; set; }
        public SmsStatusEnum Status { get; set; }
        public String ProviderStatus { get; set; }
        public Guid SagaId { get; set; }
    }
}
