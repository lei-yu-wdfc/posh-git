using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.Graydon
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Graydon.GraydonCompanyReportRequest </summary>
    [XmlRoot("GraydonCompanyReportRequest", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Graydon", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class GraydonCompanyReportRequest : MsmqMessage<GraydonCompanyReportRequest>
    {
        public String CompanyMatchIdentifier { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
