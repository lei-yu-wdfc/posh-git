using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.BusinessFixedInstallmentApplicationAdded </summary>
    [XmlRoot("BusinessFixedInstallmentApplicationAdded", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.IBusinessApplicationAdded,Wonga.Payments.PublicMessages.IPaymentsEvent" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BusinessFixedInstallmentApplicationAdded : MsmqMessage<BusinessFixedInstallmentApplicationAdded>
    {
        public DateTime CreatedOn { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Int32 Term { get; set; }
        public Decimal LoanAmount { get; set; }
        public Guid BusinessBankAccountId { get; set; }
        public Guid BusinessPaymentCardId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ProductEnum ProductId { get; set; }
        public Guid MainApplicantBankAccountId { get; set; }
        public Guid MainApplicantPaymentCardId { get; set; }
    }
}
