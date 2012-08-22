using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToPerformBalanceTransferFailedMessageResponse </summary>
    [XmlRoot("IMakeRequestToPerformBalanceTransferFailedMessageResponse", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.PpsProvider.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IMakeRequestToPerformBalanceTransferFailedMessageResponse : MsmqMessage<IMakeRequestToPerformBalanceTransferFailedMessageResponse>
    {
        public String PpsLocalTimeStamp { get; set; }
        public String ErrorMessage { get; set; }
        public Guid SagaId { get; set; }
    }
}
