using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.BusinessOrganisationExtendedIdResponse </summary>
    [XmlRoot("BusinessOrganisationExtendedIdResponse", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class BusinessOrganisationExtendedIdResponseWbUkCommand : MsmqMessage<BusinessOrganisationExtendedIdResponseWbUkCommand>
    {
        public Guid OrganisationId { get; set; }
        public String ExtendedIdentifier { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
