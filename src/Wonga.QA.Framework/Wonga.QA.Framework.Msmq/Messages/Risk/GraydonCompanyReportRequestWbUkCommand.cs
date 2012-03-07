using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Graydon.GraydonCompanyReportRequest </summary>
    [XmlRoot("GraydonCompanyReportRequest", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Graydon", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class GraydonCompanyReportRequestWbUkCommand : MsmqMessage<GraydonCompanyReportRequestWbUkCommand>
    {
        public String CompanyMatchIdentifier { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
