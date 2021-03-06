using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.BlackList
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.BlackList.BusinessApplicantBlackListRequestMessage </summary>
    [XmlRoot("BusinessApplicantBlackListRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.BlackList", DataType = "Wonga.Risk.BlackList.BlackListRequestMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BusinessApplicantBlackListRequestMessage : MsmqMessage<BusinessApplicantBlackListRequestMessage>
    {
        public Guid OrganisationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
