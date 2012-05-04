namespace Wonga.QA.Framework.CommonApi
{
    public class AccountInformationRequest: CommonApiRequest
    {
        public Authentication Authentication { get; set; }
        public Directives Directives { get; set; }
        public Applicant Applicant { get; set; }

        public CardPaymentDetails CardPaymentDetails { get; set; }
        public AgreementsToCredit AgreementsToCredit { get; set; }

        public AccountInformationRequest()
        {
            Authentication = new Authentication();
            Directives = new Directives();
            Applicant = new Applicant();
            CardPaymentDetails = new CardPaymentDetails();
            AgreementsToCredit = new AgreementsToCredit();
        }
    }

    public class Authentication
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
    }

    public class Directives
    {
        public string Environment { get; set; }
        public string RequestedAction { get; set; }
        public string CustomerReference { get; set; }
        public string AccountReference { get; set; }
        public string ReferenceDate { get; set; }
    }

    public class Applicant
    {
        public string DateOfBirth { get; set; }
        public MobilePhoneNumber MobilePhoneNumber { get; set; }

        public Applicant()
        {
            MobilePhoneNumber = new MobilePhoneNumber();
        }
    }

    public class MobilePhoneNumber
    {
        public string AreaCode { get; set; }
        public string Number { get; set; }
    }

    public class CardPaymentDetails
    {
        public string Currency { get; set; }
        public string AuthorisationAmount { get; set; }
        public RepaymentCardDetails RepaymentCardDetails { get; set; }

        public CardPaymentDetails()
        {
            RepaymentCardDetails = new RepaymentCardDetails();
        }
    }

    public class RepaymentCardDetails
    {
        public string CardType { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
    }

    public class AgreementsToCredit
    {
        public Agreement Agreement { get; set; }

        public AgreementsToCredit()
        {
            Agreement = new Agreement();
        }
    }

    public class Agreement
    {
        public string Reference { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
    }
}
