using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("GraydonCompanyReportResponse", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Graydon", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class GraydonCompanyReportResponseWbUkCommand : MsmqMessage<GraydonCompanyReportResponseWbUkCommand>
    {
        public Object GraydonReport { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
