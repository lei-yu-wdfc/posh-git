using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Email.InternalMessages.Za.SagaMessages.SendSociallyAcceptableEmailCompleteMessage </summary>
    [XmlRoot("SendSociallyAcceptableEmailCompleteMessage", Namespace = "Wonga.Email.InternalMessages.Za.SagaMessages", DataType = "Wonga.Email.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendSociallyAcceptableEmailCompleteZaCommand : MsmqMessage<SendSociallyAcceptableEmailCompleteZaCommand>
    {
        public Guid SagaId { get; set; }
    }
}
