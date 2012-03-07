using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.BusinessOrganisationExtendedIdRequest </summary>
    [XmlRoot("BusinessOrganisationExtendedIdRequest", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class BusinessOrganisationExtendedIdRequestWbUkCommand : MsmqMessage<BusinessOrganisationExtendedIdRequestWbUkCommand>
    {
        public Guid OrganisationId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
