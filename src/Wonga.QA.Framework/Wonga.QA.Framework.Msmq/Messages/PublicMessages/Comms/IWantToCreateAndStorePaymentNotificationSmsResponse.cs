using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms
{
    /// <summary> Wonga.PublicMessages.Comms.IWantToCreateAndStorePaymentNotificationSmsResponse </summary>
    [XmlRoot("IWantToCreateAndStorePaymentNotificationSmsResponse", Namespace = "Wonga.PublicMessages.Comms", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateAndStorePaymentNotificationSmsResponse : MsmqMessage<IWantToCreateAndStorePaymentNotificationSmsResponse>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
