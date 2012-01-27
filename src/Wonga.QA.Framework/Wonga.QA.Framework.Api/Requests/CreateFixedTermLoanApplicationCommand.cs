using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("CreateFixedTermLoanApplication")]
    public class CreateFixedTermLoanApplicationCommand : ApiRequest<CreateFixedTermLoanApplicationCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object PaymentCardId { get; set; }
        public Object BankAccountId { get; set; }
        public Object Currency { get; set; }
        public Object PromiseDate { get; set; }
        public Object LoanAmount { get; set; }
        public Object PromoCodeId { get; set; }
        public Object AffiliateId { get; set; }
    }
}
