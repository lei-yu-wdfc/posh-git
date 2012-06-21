using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Pl
{
    /// <summary> Wonga.Payments.Commands.Pl.CreateFixedTermLoanApplication </summary>
    [XmlRoot("CreateFixedTermLoanApplication")]
    public partial class CreateFixedTermLoanApplicationPlCommand : ApiRequest<CreateFixedTermLoanApplicationPlCommand>
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
        public Object Province { get; set; }
    }
}
