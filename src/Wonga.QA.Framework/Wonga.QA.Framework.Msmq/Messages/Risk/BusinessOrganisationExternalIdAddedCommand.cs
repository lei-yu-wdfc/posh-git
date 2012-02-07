using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("BusinessOrganisationExternalIdAdded", Namespace = "Wonga.Risk.Wb", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class BusinessOrganisationExternalIdAddedCommand : MsmqMessage<BusinessOrganisationExternalIdAddedCommand>
    {
        public Guid OrganisationId { get; set; }
        public String ExternalOrganisationId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
