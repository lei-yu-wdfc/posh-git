using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Graydon.Handlers.InternalMessages
{
    /// <summary> Wonga.Graydon.Handlers.InternalMessages.GraydonReportReadyMessage </summary>
    [XmlRoot("GraydonReportReadyMessage", Namespace = "Wonga.Graydon.Handlers.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class GraydonReportReadyMessage : MsmqMessage<GraydonReportReadyMessage>
    {
        public Int32 OrderReference { get; set; }
        public String CompanyMatchIdentifier { get; set; }
        public Guid SagaId { get; set; }
    }
}
