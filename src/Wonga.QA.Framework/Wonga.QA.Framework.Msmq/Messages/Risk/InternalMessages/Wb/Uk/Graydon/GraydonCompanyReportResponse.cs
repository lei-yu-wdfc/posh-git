using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.Graydon
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Graydon.GraydonCompanyReportResponse </summary>
    [XmlRoot("GraydonCompanyReportResponse", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Graydon", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class GraydonCompanyReportResponse : MsmqMessage<GraydonCompanyReportResponse>
    {
        public Object GraydonReport { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
