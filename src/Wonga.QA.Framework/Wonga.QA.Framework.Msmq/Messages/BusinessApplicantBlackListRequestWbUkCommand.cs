using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.BlackList.BusinessApplicantBlackListRequestMessage </summary>
    [XmlRoot("BusinessApplicantBlackListRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.BlackList", DataType = "Wonga.Risk.BlackList.BlackListRequestMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class BusinessApplicantBlackListRequestWbUkCommand : MsmqMessage<BusinessApplicantBlackListRequestWbUkCommand>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
