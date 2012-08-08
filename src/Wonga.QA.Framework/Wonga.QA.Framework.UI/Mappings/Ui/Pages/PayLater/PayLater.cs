namespace Wonga.QA.Framework.UI.Mappings.Ui.Pages.PayLater
{
    public class PayLater
    {
        public LoginPage LoginPage;
        public SubmitionPage SubmitionPage;
    }

    public class LoginPage
    {
        public string InputEmailId { get; set; }
        public string InputPassId { get; set; }
        public string SubmitButton { get; set; }
        public string Apr { get; set; }
        public string ForgottenPassword { get; set; }
        public string SingUp { get; set; }
        public string Fee { get; set; }
        public string PaymentAmount { get; set; }
    }

    public class SubmitionPage
    {
        public string FooterInfo { get; set; }
        public string AvailabelCreditCookie { get; set; }
        public string AvailabelCredit { get; set; }
        public string InfoCirculButton { get; set; }
        public string TextApproved { get; set; }
        public string RepaymentDetails { get; set; }
        public string SubmitButton { get; set; }
        public string ThanksText { get; set; }
    }
}