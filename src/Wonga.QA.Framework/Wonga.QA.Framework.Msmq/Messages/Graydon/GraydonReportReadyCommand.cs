using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Graydon
{
    [XmlRoot("GraydonReportReadyMessage", Namespace = "Wonga.Graydon.Handlers.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class GraydonReportReadyCommand : MsmqMessage<GraydonReportReadyCommand>
    {
        public Int32 OrderReference { get; set; }
        public String CompanyMatchIdentifier { get; set; }
        public Guid SagaId { get; set; }
    }
}
