using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.BusinessOrganisationExtendedIdRequest </summary>
    [XmlRoot("BusinessOrganisationExtendedIdRequest", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BusinessOrganisationExtendedIdRequest : MsmqMessage<BusinessOrganisationExtendedIdRequest>
    {
        public Guid OrganisationId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
