using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Mappings.Content;
using Wonga.QA.Framework.UI.Mappings.Content.Agreements;
using Wonga.QA.Framework.UI.Mappings.Content.Elements;
using Wonga.QA.Framework.UI.Mappings.Content.Links;
using ExtensionErrorPage = Wonga.QA.Framework.UI.Mappings.Content.ExtensionErrorPage;
using TopupDealDonePage = Wonga.QA.Framework.UI.Mappings.Content.TopupDealDonePage;
using Wonga.QA.Framework.UI.Mappings.Content.Sections;


namespace Wonga.QA.Framework.UI
{
    public class ContentMap
    {
        private static Dictionary<CultureInfo, ContentMap> Contents = new Dictionary<CultureInfo, ContentMap>();
        private static object _lock = new object();
        private static CultureInfo _cultureInfo;

        public static ContentMap Get
        {
            get
            {
                lock (_lock)
                {
                    _cultureInfo = CultureInfo.GetCultureInfo("en-US");//Thread.CurrentThread.CurrentUICulture);
                    if (!Contents.ContainsKey(_cultureInfo))
                        Contents.Add(_cultureInfo, new ContentMap(_cultureInfo));
                }

                return Contents[_cultureInfo];
            }
        }

        private string _xmlFileName;
        private XmlMapper _xmlMapper;

        public ContentMap(CultureInfo cultureInfo)
        {
            _xmlFileName =
                string.Format(string.Format("Wonga.QA.Framework.UI.Mappings.Xml.Content.{0}.{1}.xml", Config.AUT,
                                            cultureInfo.TwoLetterISOLanguageName));
            _xmlMapper = new XmlMapper(_xmlFileName, _xmlFileName);
            _xmlMapper.GetValues(this, null);
        }

#region Content
        public String YourDetails { get; set; }
        public String ProblemProcessingDetailsMessage { get; set; }
        public String PasswordWarningMessage { get; set; }
        public String ApplicationErrorMessage { get; set; }
        public String NotValidPostcode { get; set; }
        public LoanAgreement LoanAgreement { get; set; }
        public TabsElementMobile TabsElementMobile { get; set; }
        public FAQPageLinks FAQPageLinks { get; set; }
        public MobilePinVerificationSection MobilePinVerificationSection { get; set; }
        public MySummaryPage MySummaryPage { get; set; }
        public AddressDetailsPage AddressDeatailsPage { get; set; }
        public MyPaymentsPage MyPaymentsPage { get; set; }
        public RepayEarlyPaymentFailedPage RepayEarlyPaymentFailedPage { get; set; }
        public RepayDuePaymentFailedPage RepayDuePaymentFailedPage { get; set; }
        public RepayOverduePaymentFailedPage RepayOverduePaymentFailedPage { get; set; }
        public RepayEarlyPartpaySuccessPage RepayEarlyPartpaySuccessPage { get; set; }
        public RepayDuePartpaySuccessPage RepayDuePartpaySuccessPage { get; set; }
        public RepayOverduePartpaySuccessPage RepayOverduePartpaySuccessPage { get; set; }
        public RepayEarlyFullpaySuccessPage RepayEarlyFullpaySuccessPage { get; set; }
        public RepayDueFullpaySuccessPage RepayDueFullpaySuccessPage { get; set; }
        public RepayOverdueFullpaySuccessPage RepayOverdueFullpaySuccessPage { get; set; }
        public ExtensionAgreementPage ExtensionAgreementPage { get; set; }
        public L0DeclinedPage L0DeclinedPage { get; set; }
        public TimeoutTestPage TimeoutTestPage { get; set; }
        public YourDetailsSection YourDetailsSection { get; set; }
        public AccountDetailsSection AccountDetailsSection { get; set; }
        public HelpElement HelpElement { get; set; }
        public AboutUsPage AboutUsPage { get; set; }
        public TopupDealDonePage TopupDealDonePage { get; set; }
        public ExtensionErrorPage ExtensionErrorPage { get; set; }
        public ApplicationSection ApplicationSection { get; set; }
        public PrepaidBalanceBlock PrepaidBalanceBlock { get; set; }
        public ExtensionProcessingPage ExtensionProcessingPage { get; set; }
        public RepayProcessingPage RepayProcessingPage { get; set; }
        public TopupProcessingPage TopupProcessingPage { get; set; }
        public RepayErrorPage RepayErrorPage { get; set; }
        public RepaymentOptionsPage RepaymentOptionsPage { get; set; }
        public AccountSetupPage AccountSetupPage { get; set; }
#endregion
    }
}

