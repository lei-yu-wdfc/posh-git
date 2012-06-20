using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.BlackList
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.BlackList.BusinessApplicantBlackListResponseMessage </summary>
    [XmlRoot("BusinessApplicantBlackListResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.BlackList", DataType = "Wonga.Risk.BlackList.BlackListResponseMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class BusinessApplicantBlackListResponseWbUkCommand : MsmqMessage<BusinessApplicantBlackListResponseWbUkCommand>
    {
        public Boolean PresentInBlackList { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
