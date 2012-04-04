using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings.Elements;
using Wonga.QA.Framework.UI.Mappings.Pages;
using Wonga.QA.Framework.UI.Mappings.Pages.Wb;
using Wonga.QA.Framework.UI.Mappings.Sections;
using Wonga.QA.Framework.UI.Mappings.Pages.SalesForce;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using AboutUsPage = Wonga.QA.Framework.UI.Mappings.Pages.AboutUsPage;
using AcceptedPage = Wonga.QA.Framework.UI.Mappings.Pages.AcceptedPage;
using AccountDetailsPage = Wonga.QA.Framework.UI.Mappings.Pages.AccountDetailsPage;
using AddressDetailsPage = Wonga.QA.Framework.UI.Mappings.Pages.AddressDetailsPage;
using ApplyPage = Wonga.QA.Framework.UI.Mappings.Pages.ApplyPage;
using BlogPage = Wonga.QA.Framework.UI.Mappings.Pages.BlogPage;
using DealDonePage = Wonga.QA.Framework.UI.Mappings.Pages.DealDonePage;
using DeclinedPage = Wonga.QA.Framework.UI.Mappings.Pages.DeclinedPage;
using ForgotPasswordPage = Wonga.QA.Framework.UI.Mappings.Pages.ForgotPasswordPage;
using JargonBusterPage = Wonga.QA.Framework.UI.Mappings.Pages.JargonBusterPage;
using LoginPage = Wonga.QA.Framework.UI.Mappings.Pages.LoginPage;
using MyPaymentsPage = Wonga.QA.Framework.UI.Mappings.Pages.MyPaymentsPage;
using MyPersonalDetailsPage = Wonga.QA.Framework.UI.Mappings.Pages.MyPersonalDetailsPage;
using MySummaryPage = Wonga.QA.Framework.UI.Mappings.Pages.MySummaryPage;
using OurCustomersPage = Wonga.QA.Framework.UI.Mappings.Pages.OurCustomersPage;
using PersonalDebitCardPage = Wonga.QA.Framework.UI.Mappings.Pages.PersonalDebitCardPage;
using PersonalDetailsPage = Wonga.QA.Framework.UI.Mappings.Pages.PersonalDetailsPage;
using ProcessingPage = Wonga.QA.Framework.UI.Mappings.Pages.ProcessingPage;
using ResponsibleLendingPage = Wonga.QA.Framework.UI.Mappings.Pages.ResponsibleLendingPage;
using WhyUseUsPage = Wonga.QA.Framework.UI.Mappings.Pages.WhyUseUsPage;

namespace Wonga.QA.Framework.UI.Mappings
{
    public class Ui
    {
        private static Ui MyElements;
        private static object _lock = new object();

        public static Ui Get
        {
            get
            {
                lock (_lock)
                {
                    if (MyElements == null)
                    {
                        MyElements = new Ui();
                    }
                }
                return MyElements;
            }
        }

        protected Ui()
        {
            XmlMapper = new XmlMapper("Wonga.QA.Framework.UI.Mappings.Xml.Ui._base.xml", string.Format("Wonga.QA.Framework.UI.Mappings.Xml.Ui.{0}.xml", Config.AUT));
            XmlMapper.GetValues(this, null);
        }

        public XmlMapper XmlMapper = null;

        #region Section
        public virtual YourNameSection YourNameSection { get; set; }
        public virtual YourDetailsSection YourDetailsSection { get; set; }
        public virtual MobilePinVerificationSection MobilePinVerificationSection { get; set; }
        public virtual ContactingYouSection ContactingYouSection { get; set; }
        public virtual AccountDetailsSection AccountDetailsSection { get; set; }
        public virtual DebitCardSection DebitCardSection { get; set; }
        public virtual BankAccountSection BankAccountSection { get; set; }
        public virtual EmploymentDetailsSection EmploymentDetailsSection { get; set; }
        public virtual ProvinceSection ProvinceSection { get; set; }
        public virtual MyAccountNavigationSection MyAccountNavigationSection { get; set; }
        public virtual ApplicationSection ApplicationSection { get; set; }
        #endregion

        #region Elements
        public virtual SlidersElement SlidersElement { get; set; }
        public virtual LoginElement LoginElement { get; set; }
        public virtual HelpElement HelpElement { get; set; }
        public virtual InternationalElement InternationalElement { get; set; }
        public virtual FAQElement FAQElement { get; set; }
        public virtual SurveyElement SurveyElement { get; set; }
        public virtual ContactElement ContactElement { get; set; }
        public virtual TabsElement TabsElement { get; set; }

        #endregion

        #region WbPages

        public virtual EligibilityQuestionsPage EligibilityQuestionsPage { get; set; }
        public virtual BusinessDetailsPage BusinessDetailsPage { get; set; }
        public virtual AdditionalDirectorsPage AdditionalDirectorsPage { get; set; }
        public virtual AddAditionalDirectorsPage AddAditionalDirectorsPage { get; set; }
        public virtual BusinessDebitCardPage BusinessDebitCardPage { get; set; }

        #endregion

        #region CommomPages

        public virtual PersonalBankAccountDetailsPage PersonalBankAccountPage { get; set; }
        public virtual PersonalDebitCardPage PersonalDebitCardDetailsPage { get; set; }
        public virtual AcceptedPage AcceptedPage { get; set; }
        public virtual ApplyPage ApplyPage { get; set; }
        public virtual PersonalDetailsPage PersonalDetailsPage { get; set; }
        public virtual ProcessingPage ProcessingPage { get; set; }
        public virtual DealDonePage DealDonePage { get; set; }
        public virtual DeclinedPage DeclinedPage { get; set; }
        public virtual AddressDetailsPage AddressDetailsPage { get; set; }
        public virtual AccountDetailsPage AccountDetailsPage { get; set; }
        public virtual BankAccountPage BankAccountPage { get; set; }
        public virtual DebitCardPage DebitCardPage { get; set; }
        public virtual LoginPage LoginPage { get; set; }
        public virtual ForgotPasswordPage ForgotPasswordPage { get; set; }
        public virtual HomePage HomePage { get; set; }
        public virtual MySummaryPage MySummaryPage { get; set; }
        public virtual MyPaymentsPage MyPaymentsPage { get; set; }
        public virtual MyPersonalDetailsPage MyPersonalDetailsPage { get; set; }
        public virtual JargonBusterPage JargonBusterPage { get; set; }
        public virtual AboutUsPage AboutUsPage { get; set; }
        public virtual BlogPage BlogPage { get; set; }
        public virtual OurCustomersPage OurCustomersPage { get; set; }
        public virtual ResponsibleLendingPage ResponsibleLendingPage { get; set; }
        public virtual WhyUseUsPage WhyUseUsPage { get; set; }
        public virtual TopupAgreementPage TopupAgreementPage { get; set; }
        public virtual TopupDealDonePage TopupDealDonePage { get; set; }
        #endregion

        #region SalesForcePages

        public virtual SalesForceLoginPage SalesForceLoginPage { get; set; }
        public virtual SalesForceHomePage SalesForceHomePage { get; set; }
        public virtual SalesForceSearchResultPage SalesForceSearchResultPage { get; set; }
        public virtual SalesForceCustomerDetailPage SalesForceCustomerDetailPage { get; set; }
        
        #endregion
    }
}
