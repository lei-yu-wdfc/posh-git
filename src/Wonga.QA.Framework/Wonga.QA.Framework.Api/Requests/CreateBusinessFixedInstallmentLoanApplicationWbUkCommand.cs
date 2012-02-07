using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("CreateBusinessFixedInstallmentLoanApplication")]
    public partial class CreateBusinessFixedInstallmentLoanApplicationWbUkCommand : ApiRequest<CreateBusinessFixedInstallmentLoanApplicationWbUkCommand>
    {
        public Object AccountId { get; set; }
        public Object OrganisationId { get; set; }
        public Object ApplicationId { get; set; }
        public Object BusinessPaymentCardId { get; set; }
        public Object BusinessBankAccountId { get; set; }
        public Object Currency { get; set; }
        public Object NumberOfWeeks { get; set; }
        public Object LoanAmount { get; set; }
        public Object PromoCodeId { get; set; }
        public Object AffiliateId { get; set; }
    }
}
